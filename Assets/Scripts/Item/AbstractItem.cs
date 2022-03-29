using UnityEngine;
using UnityEngine.UIElements;

public abstract class AbstractItem : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public Texture2D itemIcon;

    public VisualElement visualElement;

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    // applies stats to player (can also be used to give special abilities to items)
    public abstract void ApplyStats();
}