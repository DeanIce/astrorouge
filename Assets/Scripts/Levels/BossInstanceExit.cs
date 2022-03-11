using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossInstanceExit : MonoBehaviour
{
    public string levelSceneName;
    public int levelNumber;
    private void OnTriggerEnter(Collider other)
    {
        // CONVENTION, -1 INDICATES A WIN
        if (levelNumber != -1)
        {
            // 9 is current player layer, update if change
            print("Loading " + levelSceneName);
            if (other.gameObject.GetComponent<PlayerDefault>())
            {
                EventManager.Instance.requestedScene = levelNumber;
                SceneManager.LoadScene(levelSceneName);
            }
        } 
        else
        {
            // 9 is current player layer, update if change
            print("Loading " + levelSceneName);
            if (other.gameObject.GetComponent<PlayerDefault>())
            {
                EventManager.Instance.Win();
            }
        }
    }

    /*void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene Loaded");
        LevelManager.Instance.current = levelNumber;
        LevelManager.Instance.StartCoroutineLoadLevel();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/
}
