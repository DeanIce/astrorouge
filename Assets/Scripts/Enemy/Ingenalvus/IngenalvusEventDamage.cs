using UnityEngine;

namespace Enemy.Ingenalvus
{
    /// <summary>
    ///     Script called from the AnimationController attached
    ///     to Ingenalvus. Used to start events at keyframes.
    /// </summary>
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
            ing.EndWeakPoints();
        }
    }
}