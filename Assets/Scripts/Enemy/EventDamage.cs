using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDamage : MonoBehaviour
{
    private GameObject body;
    private float attackRange;
    private const float epsilon = 0.1f; //avoid range bugs with this script being on child object
    private const float flowerEpsilon = 0.3f; //flower is stupid
    private const float flowerHeight = 1f; //flower is stupid
    private BasicEnemyAgent enemy;
    [SerializeField] private int damage;
    [SerializeField] private int damage2;

    private void Start()
    {
        enemy = GetComponentInParent<BasicEnemyAgent>();
        body = enemy.Body;
        attackRange = enemy.AttackRange;

        damage *= (Managers.LevelSelect.Instance.requestedLevel + 1);
        damage2 *= (Managers.LevelSelect.Instance.requestedLevel + 1);
    }

    public void DoDamage()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position, body.transform.forward, attackRange + epsilon, LayerMask.GetMask("Player"));
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

        hits = Physics.RaycastAll(transform.position, body.transform.forward, attackRange + epsilon, LayerMask.GetMask("Player"));
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

    public void DoDamageFlower()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position + flowerHeight * transform.up, body.transform.forward, attackRange + flowerEpsilon, LayerMask.GetMask("Player"));
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

    public void DoDamageFlower2()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position + flowerHeight * transform.up, body.transform.forward, attackRange + flowerEpsilon, LayerMask.GetMask("Player"));
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
