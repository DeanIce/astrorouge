using UnityEngine;

public class IngenalvusEventDamage : MonoBehaviour
{
    private IngenalvusAttacks ia;
    private Ingenalvus ing;

    private void Start()
    {
        ing = GetComponentInParent<Ingenalvus>();
        ia = GetComponentInParent<IngenalvusAttacks>();
    }

    public void Display(int n)
    {
        ing.DisplayWeakPoints(n);
    }

    public void BreathFireStart()
    {
        ia.BreathFireStart();
    }

    public void BreathFireStop()
    {
        ia.BreathFireStop();
    }
}