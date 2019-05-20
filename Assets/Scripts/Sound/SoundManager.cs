using System;
using System.Collections.Generic;
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

    public void Play(string name, float volume = 1f)
    {
        GameObject audioObject;

        if (audioClipDic.ContainsKey(name)) {
            audioObject = new GameObject(name);
            audioObject.transform.parent = transform;
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.clip = audioClipDic[name];
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("audio clip not present in dic");
        }
    }

    public void Pause(string name)
    {
        GameObject audioObject = transform.Find(name).gameObject;
        
        if(audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.Pause();
        }
    }

    public void Stop(string name)
    {
        GameObject audioObject = transform.Find(name).gameObject;

        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.Stop();

            Destroy(audioObject);
        }
    }
}

