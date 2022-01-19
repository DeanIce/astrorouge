using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  ================================= AUDIO MANAGER ======================================    

    The bulk of this Audio Manager was heavily designed with the help of this 
    YouTube tutorial (https://www.youtube.com/watch?v=tLyj02T51Oc&t=326s&ab_channel=Epitome) by Epitome.

    Date Created: 1/17/2022
    Name: Justin Holderby
    
    ====================================================================================== */

// Singleton Desgin Pattern for AudioManager. Need to attach script to empty object in starting scene, then an instance of AudioManager will persist 'globaly'.
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    instance = new GameObject("Audio Manager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private AudioSource music1;
    private AudioSource music2;
    private AudioSource sfx;
    private bool isMuted;

    // Determines which music source is playing. If true, music1 is playing, if false, music2 is playing.
    private bool currentMusicSource;

    private void Awake()
    {
        // Don't destroy this instance!
        DontDestroyOnLoad(this.gameObject);

        // Construct audio sources with references.
        music1 = this.gameObject.AddComponent<AudioSource>();
        music2 = this.gameObject.AddComponent<AudioSource>();
        sfx = this.gameObject.AddComponent<AudioSource>();

        // Loop variables.
        music1.loop = true;
        music2.loop = true;
    }

    // Plays music immediately, with no fade in or effect.
    public void PlayMusic(AudioClip musicClip)
    {
        // Determine which music track is active.
        AudioSource activeMusic = (currentMusicSource) ? music1 : music2;

        activeMusic.clip = musicClip;
        activeMusic.volume = 1;
        activeMusic.Play();
    }

    // If you want to switch music tracks, this will fade the current one out, and then fade 'musicClip' in at a speed of 'transitionPeriod'.
    public void PlayMusicWithCrossfade(AudioClip musicClip, float transitionPeriod = 2.0f)
    {
        // Determine which music track is active.
        AudioSource activeMusic = (currentMusicSource) ? music1 : music2;
        AudioSource newMusic = (!currentMusicSource) ? music1 : music2;

        // Swap active music.
        currentMusicSource = !currentMusicSource;

        newMusic.clip = musicClip;
        newMusic.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeMusic, newMusic, transitionPeriod));
    }

    // Coroutine method that runs iteratively over a period of time. It is not required to complete this process in only one frame, like other methods.
    private IEnumerator UpdateMusicWithCrossFade(AudioSource activeMusic, AudioSource newMusic, float transisitonPeriod)
    {
        float time = 0.0f;

        for (time = 0.0f; time <= transisitonPeriod; time += Time.deltaTime)
        {
            activeMusic.volume = (1 - (time / transisitonPeriod));
            newMusic.volume = (time / transisitonPeriod);
            yield return null;
        }

        activeMusic.Stop();
    }

    public void FadeOut(float transitionPeriod = 2.0f)
    {
        // Determine which music track is active.
        AudioSource activeMusic = (currentMusicSource) ? music1 : music2;
        StartCoroutine(UpdateFadeOut(activeMusic, transitionPeriod));
    }

    private IEnumerator UpdateFadeOut(AudioSource activeMusic, float transisitonPeriod)
    {
        float time = 0.0f;

        for (time = 0.0f; time <= transisitonPeriod; time += Time.deltaTime)
        {
            activeMusic.volume = (1 - (time / transisitonPeriod));
            yield return null;
        }

        activeMusic.Stop();
    }

    public void FadeIn(AudioClip musicClip, float transitionPeriod = 2.0f)
    {
        AudioSource newMusic = (currentMusicSource) ? music1 : music2;
        newMusic.clip = musicClip;
        newMusic.Play();
        StartCoroutine(UpdateFadeIn(newMusic, transitionPeriod));
    }

    private IEnumerator UpdateFadeIn(AudioSource newMusic, float transisitonPeriod)
    {
        float time = 0.0f;

        for (time = 0.0f; time <= transisitonPeriod; time += Time.deltaTime)
        {
            newMusic.volume = (time / transisitonPeriod);
            yield return null;
        }
    }

    // Play sound effect.
    public void PlaySFX(AudioClip sfxClip)
    {
        sfx.PlayOneShot(sfxClip);
    }
    
    // Volume can be adjusted with this overloaded method. The volume attribute should be [0 , 1].
    public void PlaySFX(AudioClip sfxClip, float volume)
    {
        sfx.PlayOneShot(sfxClip, volume);
    }

    // Modify Volume for music.
    public void SetMusicVolume(float volume)
    {
        music1.volume = volume;
        music2.volume = volume;
    }

    // Modify Volume for sound effects.
    public void SetSFXVolume(float volume)
    {
        sfx.volume = volume;
    }

    // Terminate all audioclips.
    public void StopAudio()
    {
        music1.Stop();
        music2.Stop();
        sfx.Stop();
    }

    public void ToggleMute()
    {
        if (!isMuted)
        {
            music1.mute = music2.mute = sfx.mute = isMuted = true;
        }
        else
        {
            music1.mute = music2.mute = sfx.mute = isMuted = false;
        }
    }
}