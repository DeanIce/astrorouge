using Managers;
using UnityEngine;

public class BossInstanceEnter : MonoBehaviour
{
    public string bossSceneName;

    private bool isLoading;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDefault>() && !isLoading)
        {
            EventManager.Instance.LoadBoss(bossSceneName);
            isLoading = true;
        }
    }
}