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
    // void Start()
    // {
    //     PlayMainThemeBacksound();
    // }

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

    public void StopBacksound()
    {
        if (backsoundSource.isPlaying)
        {
            backsoundSource.Stop();
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
    public void PlayMainThemeBacksound() => PlayBacksound("Main Theme");
    public void PlayWinBacksound() => PlayBacksound("Main Theme");
    public void PlayTimerBacksoundHard() => PlayBacksound("Backsound Easy");
    public void PlayTimerBacksoundMedium() => PlayBacksound("Backsound Medium");
    public void PlayTimerBacksoundEasy() => PlayBacksound("Backsound Easy");


    // Play SFX
    public void PlayTimerSFXPanic() => PlaySFX("SFX Panic");
    public void PlayClickButtonSFX() => PlaySFX("Click Button");
    public void PlayUnCorrectSFX() => PlaySFX("SFX UnCorrect");
    public void PlayCorrectSFX() => PlaySFX("SFX Correct");
}