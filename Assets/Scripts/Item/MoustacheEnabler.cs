using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoustacheEnabler : MonoBehaviour
{
    private void Start()
    {
        PlayerStats.Instance.MoustacheEnable += ManageMoustache;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.MoustacheEnable -= ManageMoustache;
    }

    private void ManageMoustache()
    {
        if (!GetComponent<MeshRenderer>().enabled)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            Vector3 originalScale = transform.localScale;
            transform.localScale = new Vector3(originalScale.x * 1.1f, originalScale.y * 1.1f, originalScale.z);
        }
    }
}
