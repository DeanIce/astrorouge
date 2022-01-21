using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySFX(sampleSoundEffect);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlayMusic(sampleMusic1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.Instance.PlayMusic(sampleMusic2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.Instance.PlayMusicWithCrossfade(sampleMusic1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AudioManager.Instance.PlayMusicWithCrossfade(sampleMusic2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AudioManager.Instance.FadeIn(sampleMusic1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AudioManager.Instance.FadeIn(sampleMusic2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AudioManager.Instance.FadeOut();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AudioManager.Instance.StopAudio();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.ToggleMute();
        }
    }
}
