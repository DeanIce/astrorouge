using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// don't allow merge yet
public class HoldToPickUp : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float pickupTime = 1f;

    [SerializeField] private float distance;

    [SerializeField] private RectTransform pickupImageRoot;

    [SerializeField] private Image pickupProgressImage;

    [SerializeField] private TextMeshProUGUI itemNameText;

    private float currentPickupTimerElapsed;

    private Inventory inventory;

    private AbstractItem itemBeingPickedUp;

    private InputAction pickup;

    private bool isKeyDown;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable() {
        var playerInputMap = InputManager.inputActions.Player;
        pickup = playerInputMap.Pickup;
        // playerInputMap.Pickup.started += PickupStarted;
        playerInputMap.Pickup.performed += PickupPerformed;
        pickup.Enable();
    }

    private void OnDisable() {
        var playerInputMap = InputManager.inputActions.Player;
        pickup = playerInputMap.Pickup;
        // playerInputMap.Pickup.started -= PickupStarted;
        playerInputMap.Pickup.performed -= PickupPerformed;
        pickup.Disable();
    }

    // public void PickupStarted(InputAction.CallbackContext obj) {
    //     isKeyDown = true;
    //     print("key down");
    // }

    public void PickupPerformed(InputAction.CallbackContext obj) {
        isKeyDown = !isKeyDown;
        print(isKeyDown);
    }

    private void Update()
    {
        SelectItemBeingPickedupFromRay();

        if (HasItemTargetted())
        {
            pickupImageRoot.gameObject.SetActive(true);


            if (isKeyDown) // change to work with new input system
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
        if (Physics.Raycast(start, transform.forward, out hitInfo, distance, layerMask))
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
        itemBeingPickedUp.ApplyStats();
        itemBeingPickedUp = null;
    }
}