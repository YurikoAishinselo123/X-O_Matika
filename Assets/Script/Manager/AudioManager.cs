using UnityEngine;
using System.Collections.Generic;
using System;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] backsounds, sfxSounds;
    public AudioSource backsoundSource, sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayTommyHappy();
    }

    public void PlayBacksound(string name)
    {
        Sound sound = Array.Find(backsounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("BackSound not found");
        }
        else
        {
            backsoundSource.clip = sound.clip;
            backsoundSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);
        if (sound == null)
        {
            Debug.Log("SFX not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }


    // Play BackSound
    public void PlayTommyHappy() => PlayBacksound("Tommy Happy");


    // Play SFX
    // public void PlayClikUI() => PlaySFX("Click UI");
}