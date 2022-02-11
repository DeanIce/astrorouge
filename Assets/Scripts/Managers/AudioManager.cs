using System.Collections;
using UnityEngine;

/*  ================================= AUDIO MANAGER ======================================    

    The bulk of this Audio Manager was heavily designed with the help of this 
    YouTube tutorial (https://www.youtube.com/watch?v=tLyj02T51Oc&t=326s&ab_channel=Epitome) by Epitome.

    Date Created: 1/17/2022
    Name: Justin Holderby
    
    ====================================================================================== */

// Singleton Desgin Pattern for AudioManager. Need to attach script to empty object in starting scene, then an instance of AudioManager will persist 'globaly'.

namespace Managers
{
    public class AudioManager : ManagerBase
    {
        // Determines which music source is playing. If true, music1 is playing, if false, music2 is playing.
        private bool currentMusicSource;
        private bool isMuted;

        private AudioSource music1;
        private AudioSource music2;
        private AudioSource sfx;

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                // Don't destroy this instance!
                DontDestroyOnLoad(gameObject);

                // Construct audio sources with references.
                music1 = gameObject.AddComponent<AudioSource>();
                music2 = gameObject.AddComponent<AudioSource>();
                sfx = gameObject.AddComponent<AudioSource>();

                // Loop variables.
                music1.loop = true;
                music2.loop = true;
            }
        }

        // Plays music immediately, with no fade in or effect.
        public void PlayMusic(AudioClip musicClip)
        {
            // Determine which music track is active.
            var activeMusic = currentMusicSource ? music1 : music2;

            activeMusic.clip = musicClip;
            activeMusic.volume = 1;
            activeMusic.Play();
        }

        // If you want to switch music tracks, this will fade the current one out, and then fade 'musicClip' in at a speed of 'transitionPeriod'.
        public void PlayMusicWithCrossfade(AudioClip musicClip, float transitionPeriod = 2.0f)
        {
            // Determine which music track is active.
            var activeMusic = currentMusicSource ? music1 : music2;
            var newMusic = !currentMusicSource ? music1 : music2;

            // Swap active music.
            currentMusicSource = !currentMusicSource;

            newMusic.clip = musicClip;
            newMusic.Play();
            StartCoroutine(UpdateMusicWithCrossFade(activeMusic, newMusic, transitionPeriod));
        }

        // Coroutine method that runs iteratively over a period of time. It is not required to complete this process in only one frame, like other methods.
        private IEnumerator UpdateMusicWithCrossFade(AudioSource activeMusic, AudioSource newMusic,
            float transisitonPeriod)
        {
            var time = 0.0f;

            for (time = 0.0f; time <= transisitonPeriod; time += Time.deltaTime)
            {
                activeMusic.volume = 1 - time / transisitonPeriod;
                newMusic.volume = time / transisitonPeriod;
                yield return null;
            }

            activeMusic.Stop();
        }

        public void FadeOut(float transitionPeriod = 2.0f)
        {
            // Determine which music track is active.
            var activeMusic = currentMusicSource ? music1 : music2;
            StartCoroutine(UpdateFadeOut(activeMusic, transitionPeriod));
        }

        private IEnumerator UpdateFadeOut(AudioSource activeMusic, float transisitonPeriod)
        {
            var time = 0.0f;

            for (time = 0.0f; time <= transisitonPeriod; time += Time.deltaTime)
            {
                activeMusic.volume = 1 - time / transisitonPeriod;
                yield return null;
            }

            activeMusic.Stop();
        }

        public void FadeIn(AudioClip musicClip, float transitionPeriod = 2.0f)
        {
            var newMusic = currentMusicSource ? music1 : music2;
            newMusic.clip = musicClip;
            newMusic.Play();
            StartCoroutine(UpdateFadeIn(newMusic, transitionPeriod));
        }

        private IEnumerator UpdateFadeIn(AudioSource newMusic, float transisitonPeriod)
        {
            var time = 0.0f;

            for (time = 0.0f; time <= transisitonPeriod; time += Time.deltaTime)
            {
                newMusic.volume = time / transisitonPeriod;
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
                music1.mute = music2.mute = sfx.mute = isMuted = true;
            else
                music1.mute = music2.mute = sfx.mute = isMuted = false;
        }
    }
}