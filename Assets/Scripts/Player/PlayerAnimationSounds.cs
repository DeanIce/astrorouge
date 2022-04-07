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

    private AudioClip[] walkingGravel;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
