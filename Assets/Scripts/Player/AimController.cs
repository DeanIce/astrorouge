using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [SerializeField] private LayerMask aimColldierMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    private void Update()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, (Screen.height / 2f)+32);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColldierMask))
        {
            debugTransform.position = raycastHit.point;
        }
    }
}
