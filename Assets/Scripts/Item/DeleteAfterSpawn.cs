using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterSpawn : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(delay());
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
