using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// don't allow merge yet
namespace Player
{
    public class HoldToPickUp : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float pickupTime = 1f;
        [SerializeField] private float distance;
        [SerializeField] private RectTransform pickupImageRoot;
        [SerializeField] private Image pickupProgressImage;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private AudioClip pickUpSoundEffect;

        private float currentPickupTimerElapsed;
        private Inventory inventory;
        private bool isKeyDown;
        private AbstractItem itemBeingPickedUp;
        private InputAction pickup;


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

        private void OnEnable()
        {
            PlayerInputActions.PlayerActions playerInputMap = InputManager.inputActions.Player;
            pickup = playerInputMap.Pickup;
            // playerInputMap.Pickup.started += PickupStarted;
            playerInputMap.Pickup.performed += PickupPerformed;
            pickup.Enable();
        }

        private void OnDisable()
        {
            PlayerInputActions.PlayerActions playerInputMap = InputManager.inputActions.Player;
            pickup = playerInputMap.Pickup;
            // playerInputMap.Pickup.started -= PickupStarted;
            playerInputMap.Pickup.performed -= PickupPerformed;
            pickup.Disable();
        }

        // public void PickupStarted(InputAction.CallbackContext obj) {
        //     isKeyDown = true;
        //     print("key down");
        // }

        public void PickupPerformed(InputAction.CallbackContext obj)
        {
            isKeyDown = !isKeyDown;
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
            float pct = currentPickupTimerElapsed / pickupTime;
            pickupProgressImage.fillAmount = pct;
        }

        private void SelectItemBeingPickedupFromRay()
        {
            Vector3 start = Camera.main.transform.position;
            Vector3 end = Camera.main.transform.forward * 15f + start;
            RaycastHit hitInfo;

            Debug.DrawLine(start, end, Color.red);
            if (Physics.Raycast(start, Camera.main.transform.forward, out hitInfo, distance, layerMask))
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

            itemBeingPickedUp.ApplyStats();
            EventManager.Instance.runStats.itemsCollected.Add(itemBeingPickedUp.itemName);
            AudioManager.Instance.PlaySFX(pickUpSoundEffect, 0.2f);
            EventManager.Instance.ItemAcquired(itemBeingPickedUp);
            itemBeingPickedUp = null;
        }
    }
}