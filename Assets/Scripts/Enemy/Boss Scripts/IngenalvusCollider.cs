using UnityEngine;

public class IngenalvusCollider : MonoBehaviour
{
    public Ingenalvus ingenalvus;

    private float damage = 10f;

    private LayerMask mask = LayerMask.GetMask();

    // private void OnTriggerEnter(Collider other)
    // {
    //     print(other.name);
    // }

    public void PassThroughDamage(float dmg)
    {
    }
}