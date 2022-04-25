using UnityEngine;
using UnityEngine.UIElements;
using Gravity;

public abstract class AbstractItem : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Texture2D itemIcon;
    private float gravTimer = 0.5f;
    private Rigidbody rb;

    public VisualElement visualElement;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (gravTimer > 0)
        {
            DoGravity();
            gravTimer -= Time.deltaTime;
        }
    }

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public void DoGravity()
    {
        // Gravity
        Vector3 sumForce = GravityManager.GetGravity(transform.position, out Vector3 upAxis);
        rb.AddForce(sumForce * Time.deltaTime);
        Debug.DrawLine(transform.position, sumForce, Color.blue);

        // Upright?
        rb.MoveRotation(Quaternion.FromToRotation(transform.up, upAxis) * transform.rotation);
    }

    // applies stats to player (can also be used to give special abilities to items)
    public abstract void ApplyStats();
}