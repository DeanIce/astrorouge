using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HoldToPickUp : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float pickupTime = 1f;

    [SerializeField] private RectTransform pickupImageRoot;

    [SerializeField] private Image pickupProgressImage;

    [SerializeField] private TextMeshProUGUI itemNameText;

    private float currentPickupTimerElapsed;

    private Inventory inventory;

    private AbstractItem itemBeingPickedUp;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        SelectItemBeingPickedupFromRay();

        if (HasItemTargetted())
        {
            pickupImageRoot.gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.E)) // change to work with new input system
                IncrementPickupProgressAndTryComplete();
            else
                currentPickupTimerElapsed = 0f;
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
        if (currentPickupTimerElapsed >= pickupTime) MoveItemToInventory();
    }

    private void UpdatePickupProgressImage()
    {
        var pct = currentPickupTimerElapsed / pickupTime;
        pickupProgressImage.fillAmount = pct;
    }

    private void SelectItemBeingPickedupFromRay()
    {
        var start = transform.position;
        var end = transform.forward * 30f;
        // Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        // Debug.DrawRay(ray.origin, ray.direction * 25f, Color.red);
        // Physics.Raycast(ray, out hitInfo, 25f, layerMask)
        RaycastHit hitInfo;

        Debug.DrawLine(start, end, Color.red);
        if (Physics.Raycast(start, transform.forward, out hitInfo, 30f, layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<AbstractItem>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
                itemNameText.text = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
                itemNameText.text = "Pickup " + itemBeingPickedUp.itemName;
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
        inventory.AddItem(itemBeingPickedUp);
        itemBeingPickedUp = null;
    }
}