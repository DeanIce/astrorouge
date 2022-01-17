using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleManager : MonoBehaviour
{
    private PersistentUpgrades data;
    public int num;

    // Start is called before the first frame update
    void Start()
    {
        data = FindObjectOfType<PersistentUpgrades>();
        num = data.num;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.E)) num++;
    }
}
