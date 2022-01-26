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
    private float pickupTime = 1f;
    [SerializeField]
    private RectTransform pickupImageRoot;
    [SerializeField]
    private Image pickupProgressImage;
    [SerializeField]
    private TextMeshProUGUI itemNameText;

    private IPickup itemBeingPickedUp;
    private float currentPickupTimerElapsed;

    private IInventory inventory;

    private void Start() {
        inventory = GetComponent<IInventory>();
    }

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
        Vector3 start = transform.position;
        Vector3 end = transform.forward * 30f;
        // Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        // Debug.DrawRay(ray.origin, ray.direction * 25f, Color.red);
        RaycastHit hitInfo;

        Debug.DrawLine(start, end, Color.red);
        if (Physics.Raycast(start, transform.forward, out hitInfo, 30f, layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<IPickup>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
                itemNameText.text = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
                itemNameText.text = "Pickup " + itemBeingPickedUp.gameObject.name;
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
        inventory.Add_Item(new IItem("dagger", inventory.item_two_background_texture));
        itemBeingPickedUp = null;
    }
}

