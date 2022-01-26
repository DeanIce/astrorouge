using UnityEngine;

public abstract class IItem : MonoBehaviour
{
    public string itemName;
    public Texture2D itemIcon;

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }
}