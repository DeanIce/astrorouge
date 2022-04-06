using UnityEngine;

public class DeleteAfterSpawn : MonoBehaviour
{
    private void Start()
    {
        print("destroy");
        Destroy(gameObject);
    }
}