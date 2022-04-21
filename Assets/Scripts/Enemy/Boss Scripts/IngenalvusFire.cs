using UnityEngine;

public class IngenalvusFire : MonoBehaviour
{
    private Ingenalvus ing;

    private void Start()
    {
        ing = transform.root.gameObject.GetComponent<Ingenalvus>();
    }


    private void OnTriggerStay(Collider other)
    {
        other.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(ing.fireDamage * Time.fixedDeltaTime);
    }

    public void Show()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }
}