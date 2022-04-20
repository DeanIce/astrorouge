using System.Collections.Generic;
using Managers;
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
public class Ingenalvus : MonoBehaviour
{
    public enum Mode
    {
        AcceptingDamage,
        WeakPoints,
        Hive,
        Dead
    }

    public GameObject player;

    public float health = 100;

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
    private NavMeshAgent agent;

    private Animator animator;

    private int weakPointsRemaining;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        weakPointsRemaining = weakPoints.Count;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (mode == Mode.AcceptingDamage)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            // print(dist);
            agent.SetDestination(player.transform.position);
            // print($"pos:{agent.updatePosition}, rot:{agent.updateRotation}");
            if (dist < offset)
            {
                print("range reached");
                agent.isStopped = true;
                // agent.updatePosition = false;
                // agent.velocity = Vector3.zero;
                agent.ResetPath();
            }
            else
            {
                agent.isStopped = false;
                // agent.updatePosition = true;
            }

            float magnitude = agent.velocity.magnitude;
            animator.SetFloat("Vertical", magnitude);
        }
    }

    public void TakeDmg(float dmg)
    {
        if (mode == Mode.AcceptingDamage)
        {
            health -= dmg;
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
                    animator.SetInteger("Weak Points", n / 2);
                    animator.SetTrigger("Take Damage");
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
        animator.SetTrigger("Die");
        agent.velocity = Vector3.zero;
        agent.enabled = false;
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
        health = 20;
        agent.isStopped = false;
    }
}