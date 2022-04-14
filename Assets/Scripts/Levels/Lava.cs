using UnityEngine;

namespace Levels
{
    public class Lava : MonoBehaviour
    {
        public float delay = 1.0f;

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

        private void OnCollisionEnter(Collision collision)
        {
            running = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            timer = 1.0f;
            running = false;
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (collisionInfo.gameObject.name == "PlayerDefault" && timer <= 0)
            {
                var sem = collisionInfo.gameObject.GetComponent<StatusEffectManager>();
                sem.ApplyBurn(3);
            }
        }
    }
}