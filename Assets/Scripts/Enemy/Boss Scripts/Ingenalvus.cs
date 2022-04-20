using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Ingenalvus : MonoBehaviour
{
    public float health = 100;

    public GameObject colliderHead;

    public int xpGift = 100;
    public bool iAmAlive = true;

    public List<IngenalvusCollider> weakPoints;

    /// <summary>
    ///     Number of damage levels to display
    /// </summary>
    public int level;

    private readonly int maxLevel = 2;

    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

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


    public void TakeDmg(float dmg)
    {
        if (health > 0)
        {
            EventManager.Instance.runStats.damageDealt += dmg;
            health -= dmg;

            level += 1;

            animator.SetInteger("Weak Points", level);
            animator.SetTrigger("Take Damage");

            print(health);
            if (health <= 0f && iAmAlive) Die();
        }
    }

    public void Die()
    {
        // Give XP for killing the enemy
        PlayerStats.Instance.xp += xpGift;
        EventManager.Instance.PlayerStatsUpdated();

        iAmAlive = false;
        EventManager.Instance.runStats.enemiesKilled++;
        animator.SetTrigger("Die");
    }
}