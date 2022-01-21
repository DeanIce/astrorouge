using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleManager : MonoBehaviour
{
    private Data data;

    // Start is called before the first frame update
    void Start()
    {
        data = PersistentUpgrades.Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PersistentUpgrades.Save(data);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.E)) data.num++;
        if (Input.GetKeyDown(KeyCode.Escape)) PersistentUpgrades.Save(data);
    }
}
