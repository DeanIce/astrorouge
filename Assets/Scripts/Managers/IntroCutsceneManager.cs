using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenuTest");
    }
}
