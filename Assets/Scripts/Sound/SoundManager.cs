using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    [HideInInspector]

    private AudioSource oneShotPlayer;

    private Dictionary<string, AudioClip> audioClipDic;

    private TaskList taskList;

    private void Awake()
    {
        oneShotPlayer = GetComponent<AudioSource>();

        audioClipDic = new Dictionary<string, AudioClip>();

        taskList = new TaskList();

        foreach (AudioClip i in audioClips)
        {
            audioClipDic.Add(i.name, i);
        }
    }

    //Play and forget, no need for audio source but can't be paused/stopped
    public void PlayOneShot(string name, float volume = 1f)
    {
        oneShotPlayer.PlayOneShot(audioClipDic[name], volume);
    }

    public AudioSource Play(string name, float volume = 1f, bool loop = false)
    {
        if(!audioClipDic.ContainsKey(name))
        {
            Debug.LogError("audio clip not present in dic: " + name);
            return null;
        } 
        Transform audioSearchResult = transform.Find(name);
        AudioSource audioSource;

        if (audioSearchResult != null)
        {
            audioSource = audioSearchResult.GetComponent<AudioSource>();
        }
        else{
            GameObject audioObject;
            audioObject = new GameObject(name);
            audioObject.transform.parent = transform;
            audioSource = audioObject.AddComponent<AudioSource>();
        }


        audioSource.clip = audioClipDic[name];
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        return audioSource;
    }

    private IEnumerator fadeInCoroutine(string name, float maxVolume = 1f, float stepSize = .005f, bool loop = false)
    {
        AudioSource player = Play(name, 0f, loop);

        if (player != null)
        {
            while (player.volume < maxVolume)
            {
                player.volume += stepSize;
                yield return null;
            }
        }
    }
    public void FadeIn(string name, float maxVolume = 1f, float stepSize = .005f, bool loop = false)
    {
        if (taskList.contains(name))
            taskList.Stop(name);

        taskList[name] = new Task(fadeInCoroutine(name, maxVolume, stepSize, loop));
    }

    public void Pause(string name)
    {
        Transform audioObject = transform.Find(name);
        
        if(audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.Pause();
        }
    }

    public void Stop(string name)
    {
        Transform audioObject = transform.Find(name);

        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.Stop();

            Destroy(audioObject.gameObject);
        }
    }

    private IEnumerator fadeOutCoroutine(string name, float stepSize = .005f)
    {
        Transform audioObject = transform.Find(name);

        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            while (audioSource.volume > 0f)
            {
                audioSource.volume -= stepSize;
                yield return null;
            }
        }

        Pause(name);
    }
    public void FadeOut(string name, float stepSize = .005f)
    {
        if (taskList.contains(name))
            taskList.Stop(name);

        taskList[name] = new Task(fadeOutCoroutine(name, stepSize));
    }

    public bool isPlaying(string name)
    {
        if (transform.Find(name) == null)
            return false;
        else if (transform.Find(name).GetComponent<AudioSource>().isPlaying)
            return true;
        else
            return false;
    }
}

