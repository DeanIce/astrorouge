using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class HandleDeath : MonoBehaviour
{
    [SerializeField] private AudioClip deathSoundEffect;
    void Start()
    {
        Debug.Log("Death Script Enabled");

        AudioManager.Instance.PlaySFX(deathSoundEffect);
        StartCoroutine(StartRecap());


        // TODO: Handle everything that happens after death.
    }

    public IEnumerator StartRecap()
    {
        yield return new WaitForSeconds(4);
        EventManager.Instance.Recap();
    }
}



