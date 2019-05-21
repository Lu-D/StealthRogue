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

    private void Awake()
    {
        oneShotPlayer = GetComponent<AudioSource>();

        audioClipDic = new Dictionary<string, AudioClip>();

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

    private IEnumerator fadeInCoroutine(string name, float maxVolume = 1f, bool loop = false)
    {
        AudioSource player = Play(name, 0f, loop);

        if (player != null)
        {
            while (player.volume < 2f)
            {
                player.volume += .005f;
                yield return null;
            }
        }
    }
    public void FadeIn(string name, float maxVolume = 1f, bool loop = false)
    {
        new Task(fadeInCoroutine(name, maxVolume, loop));
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

    private IEnumerator fadeOutCoroutine(string name)
    {
        Transform audioObject = transform.Find(name);

        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            while (audioSource.volume > 0f)
            {
                audioSource.volume -= .005f;
                yield return null;
            }
        }

        Stop(name);
    }
    public void FadeOut(string name)
    {
        new Task(fadeOutCoroutine(name));
    }

    public bool isPlaying(string name)
    {
        return transform.Find(name) != null;
    }
}

