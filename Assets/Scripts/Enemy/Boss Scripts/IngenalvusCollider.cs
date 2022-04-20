using UnityEngine;

public class IngenalvusCollider : MonoBehaviour
{
    public Ingenalvus ingenalvus;

    public bool acceptingDamage;

    private float damage = 10f;

    private LayerMask mask = LayerMask.GetMask();

    private void OnTriggerEnter(Collider other)
    {
        if (acceptingDamage)
        {
            print(other.name);
            ingenalvus.DestroyWeakPoint(this);
            Destroy(other.gameObject);
        }
    }

    public void PassThroughDamage(float dmg)
    {
        print(dmg);
    }
}