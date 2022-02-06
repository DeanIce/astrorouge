using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    bool floatUp;
    float startHeight;
    float upAndDownMax = 0.02f;
    float upAndDownSpeed = 0.02f;

    void Start()
    {
        floatUp = false;
        startHeight = transform.position.y;
    }

    void Update()
    {
        if ((transform.position.y < startHeight + upAndDownMax) && floatUp)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, (float)(transform.position.y + upAndDownSpeed * Time.deltaTime), transform.position.z), transform.rotation);
            if (transform.position.y >= startHeight + upAndDownMax)
            {
                floatUp = false;
            }
        }
        else if ((transform.position.y > startHeight - upAndDownMax) && !floatUp)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, (float)(transform.position.y - upAndDownSpeed * Time.deltaTime), transform.position.z), transform.rotation);
            if (transform.position.y <= startHeight - upAndDownMax)
            {
                floatUp = true;
            }
        }
    }
}
