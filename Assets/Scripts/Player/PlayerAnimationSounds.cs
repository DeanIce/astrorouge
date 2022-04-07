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

    // Start is called before the first frame update
    void Start()
    {
        walkGravel1 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel2 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel3 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel4 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel5 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel6 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel7 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");
        walkGravel8 = (AudioClip) Resources.Load("Audio/Sound Effects/Movement/walkGravel1");

        runGravel1 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel2 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel3 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel4 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel5 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel6 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel7 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");
        runGravel8 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runGravel1");

        walkSnow1 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow2 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow3 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow4 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow5 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow6 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow7 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");
        walkSnow8 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/walkSnow1");

        runSnow1 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow2 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow3 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow4 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow5 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow6 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow7 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");
        runSnow8 = (AudioClip)Resources.Load("Audio/Sound Effects/Movement/runSnow1");

        MakeLists();
    }
    
    private void MakeLists()
    {
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
