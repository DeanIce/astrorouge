using UnityEngine;

namespace Levels
{
    public class Slow : MonoBehaviour
    {
        public float delay = 2.0f;

        private bool running;
        private float timer;

        private void Start()
        {
            timer = delay;
        }

        private void Update()
        {
            if (running) timer -= Time.deltaTime;
        }


        private void OnTriggerEnter(Collider other)
        {
            running = true;
        }


        private void OnTriggerExit(Collider other)
        {
            timer = 1.0f;
            running = false;
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == "PlayerDefault" && timer <= 0)
            {
                var sem = other.gameObject.GetComponent<StatusEffectManager>();
                sem.ApplySlow(2);
            }
        }
    }
}