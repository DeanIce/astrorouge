using UnityEngine;

namespace Managers.Test
{
    public class AudioManagerTest : MonoBehaviour
    {
        /*  ================================= AUDIO MANAGER TEST ======================================    
    
        This file assigns inputs to specific keys in order to test the AudioManager functionality.
    
        Date Created: 1/17/2022
        Name: Justin Holderby
    
         [Key]   [Function]
         ________________________________________________________
        | Space | Play Sound Effect                              |
        |_______|________________________________________________|
        | 1     | Play Sample Music 1                            |
        |_______|________________________________________________|
        | 2     | Play Sample Music 2                            |
        |_______|________________________________________________|
        | 3     | Play Sample Music 1 w/ Crossfade               |
        |_______|________________________________________________|
        | 4     | Play Sample Music 2 w/ Crossfade               |
        |_______|________________________________________________|
        | 5     | Play Sample Music 1 w/ Fade-in                 |
        |_______|________________________________________________|  
        | 6     | Play Sample Music 2 w/ Fade-in                 |
        |_______|________________________________________________|
        | 7     | Fade-out                                       |
        |_______|________________________________________________|
        | 0     | Terminate all audio clips      *for debugging* |
        |_______|________________________________________________|
        | M     | Mute                                           |
        |_______|________________________________________________|
        
        =============================================================================================== */

        public AudioClip sampleSoundEffect;
        public AudioClip sampleMusic1;
        public AudioClip sampleMusic2;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) AudioManager.instance.PlaySFX(sampleSoundEffect);

            if (Input.GetKeyDown(KeyCode.Alpha1)) AudioManager.instance.PlayMusic(sampleMusic1);

            if (Input.GetKeyDown(KeyCode.Alpha2)) AudioManager.instance.PlayMusic(sampleMusic2);

            if (Input.GetKeyDown(KeyCode.Alpha3)) AudioManager.instance.PlayMusicWithCrossfade(sampleMusic1);

            if (Input.GetKeyDown(KeyCode.Alpha4)) AudioManager.instance.PlayMusicWithCrossfade(sampleMusic2);

            if (Input.GetKeyDown(KeyCode.Alpha5)) AudioManager.instance.FadeIn(sampleMusic1);

            if (Input.GetKeyDown(KeyCode.Alpha6)) AudioManager.instance.FadeIn(sampleMusic2);

            if (Input.GetKeyDown(KeyCode.Alpha7)) AudioManager.instance.FadeOut();

            if (Input.GetKeyDown(KeyCode.Alpha0)) AudioManager.instance.StopAudio();

            if (Input.GetKeyDown(KeyCode.M)) AudioManager.instance.ToggleMute();
        }
    }
}