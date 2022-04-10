using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSounds : MonoBehaviour
{

    private AudioClip walkGravel1;
    private AudioClip walkGravel2;
    private AudioClip walkGravel3;
    private AudioClip walkGravel4;
    private AudioClip walkGravel5;
    private AudioClip walkGravel6;
    private AudioClip walkGravel7;
    private AudioClip walkGravel8;

    private AudioClip runGravel1;
    private AudioClip runGravel2;
    private AudioClip runGravel3;
    private AudioClip runGravel4;
    private AudioClip runGravel5;
    private AudioClip runGravel6;
    private AudioClip runGravel7;
    private AudioClip runGravel8;

    private AudioClip walkSnow1;
    private AudioClip walkSnow2;
    private AudioClip walkSnow3;
    private AudioClip walkSnow4;
    private AudioClip walkSnow5;
    private AudioClip walkSnow6;
    private AudioClip walkSnow7;
    private AudioClip walkSnow8;

    private AudioClip runSnow1;
    private AudioClip runSnow2;
    private AudioClip runSnow3;
    private AudioClip runSnow4;
    private AudioClip runSnow5;
    private AudioClip runSnow6;
    private AudioClip runSnow7;
    private AudioClip runSnow8;

    private List<AudioClip> walkingGravel;
    private List<AudioClip> runningGravel;
    private List<AudioClip> walkingSnow;
    private List<AudioClip> runningSnow;

    private PlayerDefault player;
    [SerializeField] private float walkStepVolume = 0.7f;
    [SerializeField] private float runStepVolume = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        walkGravel1 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel2 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel2");
        walkGravel3 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel3");
        walkGravel4 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel4");
        walkGravel5 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel5");
        walkGravel6 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel6");
        walkGravel7 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel7");
        walkGravel8 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel8");

        runGravel1 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel2 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel2");
        runGravel3 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel3");
        runGravel4 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel4");
        runGravel5 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel5");
        runGravel6 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel6");
        runGravel7 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel7");
        runGravel8 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel8");

        walkSnow1 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow2 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow2");
        walkSnow3 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow3");
        walkSnow4 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow4");
        walkSnow5 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow5");
        walkSnow6 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow6");
        walkSnow7 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow7");
        walkSnow8 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow8");

        runSnow1 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow2 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow2");
        runSnow3 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow3");
        runSnow4 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow4");
        runSnow5 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow5");
        runSnow6 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow6");
        runSnow7 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow7");
        runSnow8 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow8");

        player = GetComponentInParent<PlayerDefault>();

        MakeLists();
    }

    private void PlayStepSound(AnimationEvent evt)
    {
        int levelNumber = LevelSelect.Instance.requestedLevel;
        if (evt.animatorClipInfo.weight > 0.5f)
        {
            if (levelNumber == 0 || levelNumber == 2)
            {
                PlayGravelFootsteps();
            }
            else
            {
                PlaySnowFootsteps();
            }
        }
    }

    private void PlayGravelFootsteps()
    {
        if (!player.IsSprinting)
        {
            AudioManager.Instance.PlaySFX(walkingGravel[Random.Range(0, walkingGravel.Count)], walkStepVolume);
        }
        else
        {
            AudioManager.Instance.PlaySFX(runningGravel[Random.Range(0, runningGravel.Count)], runStepVolume);
        }
    }

    private void PlaySnowFootsteps()
    {
        if (!player.IsSprinting)
        {
            AudioManager.Instance.PlaySFX(walkingSnow[Random.Range(0, walkingSnow.Count)], walkStepVolume);
        }
        else
        {
            AudioManager.Instance.PlaySFX(runningSnow[Random.Range(0, runningSnow.Count)], runStepVolume);
        }
    }

    private void MakeLists()
    {
        walkingGravel = new List<AudioClip>();
        runningGravel = new List<AudioClip>();
        walkingSnow = new List<AudioClip>();
        runningSnow = new List<AudioClip>();

        walkingGravel.Add(walkGravel1);
        walkingGravel.Add(walkGravel2);
        walkingGravel.Add(walkGravel3);
        walkingGravel.Add(walkGravel4);
        walkingGravel.Add(walkGravel5);
        walkingGravel.Add(walkGravel6);
        walkingGravel.Add(walkGravel7);
        walkingGravel.Add(walkGravel8);

        runningGravel.Add(runGravel1);
        runningGravel.Add(runGravel2);
        runningGravel.Add(runGravel3);
        runningGravel.Add(runGravel4);
        runningGravel.Add(runGravel5);
        runningGravel.Add(runGravel6);
        runningGravel.Add(runGravel7);
        runningGravel.Add(runGravel8);

        walkingSnow.Add(walkSnow1);
        walkingSnow.Add(walkSnow2);
        walkingSnow.Add(walkSnow3);
        walkingSnow.Add(walkSnow4);
        walkingSnow.Add(walkSnow5);
        walkingSnow.Add(walkSnow6);
        walkingSnow.Add(walkSnow7);
        walkingSnow.Add(walkSnow8);

        runningSnow.Add(runSnow1);
        runningSnow.Add(runSnow2);
        runningSnow.Add(runSnow3);
        runningSnow.Add(runSnow4);
        runningSnow.Add(runSnow5);
        runningSnow.Add(runSnow6);
        runningSnow.Add(runSnow7);
        runningSnow.Add(runSnow8);
    }
}
