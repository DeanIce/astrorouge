using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDamage : MonoBehaviour
{
    private GameObject body;
    private float attackRange;
    private BasicEnemyAgent enemy;
    [SerializeField] private int damage;
    [SerializeField] private int damage2;

    private void Start()
    {
        enemy = GetComponentInParent<BasicEnemyAgent>();
        body = enemy.Body;
        attackRange = enemy.AttackRange;
    }

    public void DoDamage()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, body.transform.forward, attackRange + 0.1f, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(damage);
                }
            }
        }
    }

    public void DoDamage2()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, body.transform.forward, attackRange + 0.1f, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            //check for the player in the things the ray hit by whether it has a PlayerDefault
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.GetComponent<PlayerDefault>() != null)
                {
                    hit.collider.gameObject.GetComponent<PlayerDefault>().TakeDmg(damage2);
                }
            }
        }
    }
}
