using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class TutorialToMenu : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDefault>()) EventManager.Instance.Menu();
    }
}
