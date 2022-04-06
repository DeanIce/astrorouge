using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerEnableDialogue : MonoBehaviour
{
    public GameObject dialogueBox;

    private bool doneDialogue = false;

    private void OnTriggerEnter(Collider other)
    {
        // Convention: Player Layer is 9
        if (!doneDialogue && other.gameObject.layer == 9)
        {
            dialogueBox.SetActive(true);
            doneDialogue = true;
        }
    }
}
