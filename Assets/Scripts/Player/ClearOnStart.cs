using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(clearArea());
    }

    private IEnumerator clearArea()
    {
        yield return new WaitForSeconds(2f);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

        foreach (Collider hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.transform.root.name);
            if (hitCollider.transform.root.CompareTag("enemy") || hitCollider.transform.root.CompareTag("Prop"))
            {
                Debug.Log("Destroying: " + hitCollider.transform.root.gameObject.name);
                Destroy(hitCollider.transform.root.gameObject);
            }
        }
    }
}
