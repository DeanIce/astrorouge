using UnityEngine;

public class IngenalvusEventDamage : MonoBehaviour
{
    private Ingenalvus ing;

    private void Start()
    {
        ing = GetComponentInParent<Ingenalvus>();
    }

    public void Display(int n)
    {
        ing.DisplayWeakPoints(n);
    }
}