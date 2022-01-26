using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float positionSpeed;
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, positionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationSpeed);
    }
}
