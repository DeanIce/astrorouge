using UnityEngine;
using UnityEngine.SceneManagement;

public class BossInstanceEnter : MonoBehaviour
{
    public string bossSceneName;

    private bool isLoading;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDefault>() && !isLoading)
        {
            SceneManager.LoadScene(bossSceneName);
            isLoading = true;
        }
    }
}