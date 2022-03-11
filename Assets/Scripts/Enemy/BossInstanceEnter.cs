using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossInstanceEnter : MonoBehaviour
{
    public string bossSceneName;
    private void OnTriggerEnter(Collider other)
    {
        // 9 is current player layer, update if change
        print("Loading " + bossSceneName);
        if (other.gameObject.GetComponent<PlayerDefault>())
        {
            SceneManager.LoadScene(bossSceneName);
        }
    }
}
