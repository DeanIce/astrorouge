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
        yield return new WaitForSeconds(1f);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 7f);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform.root.CompareTag("enemy"))
            {
                Debug.Log("Destroying: " + hitCollider.transform.root.gameObject.name);
                Destroy(hitCollider.transform.root.gameObject);
            }
            else if (hitCollider.transform.gameObject.CompareTag("Prop"))
            {
                Debug.Log("Destroying: " + hitCollider.transform.gameObject.name);
                Destroy(hitCollider.transform.gameObject);
            }
        }
    }
}
