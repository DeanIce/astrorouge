using System.Collections.Generic;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.AI;

/*
 * Steps in battle:
 *
 * Idea A:
 * - damage boss enough to trigger "damage show weak spots" events
 * - when damaged a certain amount show those animations for a short time
 * - if all get killed, die
 *
 *
 * Idea B:
 * - periodically release swarms
 */
namespace Enemy.Ingenalvus
{
    public class Ingenalvus : MonoBehaviour
    {
        public enum Mode
        {
            AcceptingDamage,
            WeakPoints,
            Hive,
            Dead
        }

        private static readonly int vertical = Animator.StringToHash("Vertical");
        private static readonly int property = Animator.StringToHash("Weak Points");
        private static readonly int property1 = Animator.StringToHash("Take Damage");
        private static readonly int die = Animator.StringToHash("Die");


        public float stageHealth = 20f;

        public GameObject portal;
        public GameObject player;

        public GameObject[] bodyColliders;


        public int xpGift = 100;

        public List<IngenalvusCollider> weakPoints;

        /// <summary>
        ///     Number of damage levels to display
        /// </summary>
        public int level;

        public Mode mode = Mode.AcceptingDamage;

        public float offset = 20;
        public float speed = 2;
        public float fireDamage = 10;

        public Animator animator;
        public float smashDamage = 40;

        public BossHealthBar bossHealthBar;
        private NavMeshAgent agent;

        private float health;

        private int weakPointsRemaining;

        private void Start()
        {
            health = stageHealth;
            bossHealthBar.SetHealth(health, stageHealth);
            animator = GetComponentInChildren<Animator>();
            weakPointsRemaining = weakPoints.Count;
            agent = GetComponent<NavMeshAgent>();
            portal.SetActive(false);
        }

        private void Update()
        {
            if (mode == Mode.AcceptingDamage)
            {
                Vector3 position = player.transform.position;
                float dist = Vector3.Distance(transform.position, position);
                agent.SetDestination(position);
                if (dist < offset)
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                }
                else
                    agent.isStopped = false;

                float magnitude = agent.velocity.magnitude;
                animator.SetFloat(vertical, magnitude);
            }
        }

        public void TakeDmg(float dmg)
        {
            if (mode == Mode.AcceptingDamage)
            {
                health -= dmg;
                bossHealthBar.SetHealth(health, stageHealth);
                if (health <= 0f)
                {
                    mode = Mode.WeakPoints;
                    foreach (GameObject bodyCollider in bodyColliders)
                    {
                        bodyCollider.SetActive(false);
                    }


                    // Set visible weak points to accept damage
                    int n = weakPoints.Count - weakPointsRemaining + 2;
                    if (weakPointsRemaining > 0)
                    {
                        animator.SetInteger(property, n / 2);
                        animator.SetTrigger(property1);
                    }
                    else
                        Die();
                }
            }
        }

        public void DisplayWeakPoints(int n)
        {
            print($"Displaying {n}/{weakPoints.Count} weak points.");
            for (var i = 0; i < n; i++)
            {
                weakPoints[i].acceptingDamage = true;
            }

            agent.isStopped = true;
            agent.ResetPath();
        }


        public void Die()
        {
            // Give XP for killing the enemy
            PlayerStats.Instance.xp += xpGift;
            EventManager.Instance.PlayerStatsUpdated();

            mode = Mode.Dead;
            EventManager.Instance.runStats.enemiesKilled++;
            animator.SetTrigger(die);
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            portal.SetActive(true);
        }

        public void DestroyWeakPoint(IngenalvusCollider ic)
        {
            print($"Weak point {ic.gameObject.name} destroyed.");

            ic.gameObject.SetActive(false);
            weakPointsRemaining--;
        }

        public void EndWeakPoints()
        {
            foreach (GameObject bodyCollider in bodyColliders)
            {
                bodyCollider.SetActive(true);
            }

            foreach (IngenalvusCollider ic in weakPoints)
            {
                ic.acceptingDamage = false;
            }

            // Set mode back
            mode = Mode.AcceptingDamage;
            health = stageHealth;
            agent.isStopped = false;
        }
    }
}