using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldToPickUp : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float pickupTime = 2f;
    [SerializeField]
    private RectTransform pickupImageRoot;
    [SerializeField]
    private Image pickupProgressImage;
    [SerializeField]
    private TextMeshProUGUI itemNameText;

    private IItem itemBeingPickedUp;
    private float currentPickupTimerElapsed;

    private void Update()
    {
        SelectItemBeingPickedupFromRay();

        if (HasItemTargetted()) {
            pickupImageRoot.gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.E)) // change to work with new input system
            {
                IncrementPickupProgressAndTryComplete();
            } 
            else
            {
                currentPickupTimerElapsed = 0f;
            }
            UpdatePickupProgressImage();
        }
        else
        {
            pickupImageRoot.gameObject.SetActive(false);
            currentPickupTimerElapsed = 0f;
        }
    }

    private bool HasItemTargetted()
    {
        return itemBeingPickedUp != null;
    }

    private void IncrementPickupProgressAndTryComplete()
    {
        currentPickupTimerElapsed += Time.deltaTime;
        if (currentPickupTimerElapsed >= pickupTime)
        {
            MoveItemToInventory();
        }
    }

    private void UpdatePickupProgressImage() {
        float pct = currentPickupTimerElapsed / pickupTime;
        pickupProgressImage.fillAmount = pct;
    }

    private void SelectItemBeingPickedupFromRay() {
        Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 3f, layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<IItem>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
                itemNameText.text = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
                itemNameText.text = "Pickup " + itemBeingPickedUp.item_name;
            }
        }
        else
        {
            itemBeingPickedUp = null;
            itemNameText.text = null;
        }
    }
    private void MoveItemToInventory()
    {
        Destroy(itemBeingPickedUp.gameObject);
        itemBeingPickedUp = null;
    }
}

