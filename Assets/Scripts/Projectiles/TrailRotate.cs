using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0f, 1500f, 0f) * Time.deltaTime);
    }
}
