using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] Vector2 look;
    [SerializeField] private float sensitivity = .5f;

    void Update()
    {
        look.y += Input.GetAxis("Mouse Y") * sensitivity;
        look.x += Input.GetAxis("Mouse X") * sensitivity;
        transform.localRotation = Quaternion.Euler(-look.y, look.x, 0);
    }
}
