using Managers;
using UnityEngine;

public class BossInstanceExit : MonoBehaviour
{
    public string levelSceneName;
    public int levelNumber;
    public AudioClip bossMusic;

    private void Start()
    {
        AudioManager.Instance.PlayMusicWithCrossfade(bossMusic);
    }

    private void OnTriggerEnter(Collider other)
    {
        // CONVENTION, -1 INDICATES A WIN
        if (levelNumber != -1)
        {
            // 9 is current player layer, update if change
            print("Loading " + levelSceneName);
            if (other.gameObject.GetComponent<PlayerDefault>()) EventManager.Instance.LoadLevel(levelNumber);
        }
        else
        {
            // 9 is current player layer, update if change
            print("Loading " + levelSceneName);
            if (other.gameObject.GetComponent<PlayerDefault>()) EventManager.Instance.Win();
        }
    }


}