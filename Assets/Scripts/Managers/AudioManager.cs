using System.Collections;
using UnityEngine;

namespace Managers
{
    /// <summary>
    ///     The bulk of this Audio Manager was heavily designed with the help of this
    ///     YouTube tutorial(https: //www.youtube.com/watch?v=tLyj02T51Oc&t=326s&ab_channel=Epitome) by Epitome.
    ///     Date Created:
    ///     1 / 17 / 2022
    ///     Name:
    ///     Justin Holderby
    /// </summary>
    public class AudioManager : ManagerSingleton<AudioManager>
    {
        public AudioClip buttonClick;

        // Determines which music source is playing. If true, music1 is playing, if false, music2 is playing.
        private bool currentMusicSource;
        private bool isMuted;

        private AudioSource music1;
        private AudioSource music2;
        private AudioSource sfx;


        private void Start()
        {
            // Construct audio sources with references.
            music1 = gameObject.AddComponent<AudioSource>();
            music2 = gameObject.AddComponent<AudioSource>();
            sfx = gameObject.AddComponent<AudioSource>();

            // Loop variables.
            music1.loop = true;
            music2.loop = true;
        }

        private void OnEnable()
        {
            EventManager.Instance.settingsUpdated += UpdateLevels;
        }

        private void OnDisable()
        {
            EventManager.Instance.settingsUpdated -= UpdateLevels;
        }

        private void UpdateLevels(UserSettings settings)
        {
            LOG($"music: {settings.volumeMusic}, game: {settings.volumeGame}");
            SetMusicVolume(settings.volumeMusic);
            SetSFXVolume(settings.volumeGame);
        }

        /// <summary>
        ///     Plays music immediately, with no fade in or effect.
        /// </summary>
        /// <param name="musicClip"></param>
        public void PlayMusic(AudioClip musicClip)
        {
            // Determine which music track is active.
            var activeMusic = currentMusicSource ? music1 : music2;

            activeMusic.clip = musicClip;
            activeMusic.volume = 1;
            activeMusic.Play();
        }

        /// <summary>
        ///     If you want to switch music tracks, this will fade the current one out, and then fade 'musicClip' in at a speed
        ///     of 'transitionPeriod'.
        /// </summary>
        /// <param name="musicClip"></param>
        /// <param name="transitionPeriod"></param>
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

        /// <summary>
        ///     Coroutine method that runs iteratively over a period of time. It is not required to complete this process in only
        ///     one frame, like other methods.
        /// </summary>
        /// <param name="activeMusic"></param>
        /// <param name="newMusic"></param>
        /// <param name="transisitonPeriod"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Play sound effect.
        /// </summary>
        /// <param name="sfxClip"></param>
        public void PlaySFX(AudioClip sfxClip)
        {
            sfx.PlayOneShot(sfxClip);
        }

        /// <summary>
        ///     Volume can be adjusted with this overloaded method. The volume attribute should be [0 , 1].
        /// </summary>
        /// <param name="sfxClip"></param>
        /// <param name="volume"></param>
        public void PlaySFX(AudioClip sfxClip, float volume)
        {
            sfx.PlayOneShot(sfxClip, volume);
        }

        /// <summary>
        ///     Modify Volume for music.
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            music1.volume = volume;
            music2.volume = volume;
            LOG(music1.volume + " " + music2.volume);
        }

        /// <summary>
        ///     Modify Volume for sound effects.
        /// </summary>
        /// <param name="volume"></param>
        public void SetSFXVolume(float volume)
        {
            sfx.volume = volume;
        }

        /// <summary>
        ///     Terminate all audioclips.
        /// </summary>
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
}