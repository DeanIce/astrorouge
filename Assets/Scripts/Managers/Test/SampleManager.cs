using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Managers
{
    public class SampleManager : MonoBehaviour
    {
        // E to update, Escape to save, O to print current saved value.
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                print("Increment.");
                data.num++;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("Saved.");
                PersistentUpgrades.Save(data);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                print($"Current: {PersistentUpgrades.Load().num}");
            }
        }
    }

}
