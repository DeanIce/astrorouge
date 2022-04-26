using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity;

public class GravityForItems : MonoBehaviour
{
    private float gravTimer = 0.5f;
    private Rigidbody rb;
    [SerializeField] Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gravTimer > 0)
        {
            DoGravity();
            gravTimer -= Time.deltaTime;
        }
        else
        {
            coll.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void DoGravity()
    {
        // Gravity
        Vector3 sumForce = GravityManager.GetGravity(transform.position, out Vector3 upAxis);
        rb.AddForce(sumForce * Time.deltaTime);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        // Upright?
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) * transform.rotation);
    }
}
