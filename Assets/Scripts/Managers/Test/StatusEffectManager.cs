using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private IEnemy enemy;
    public List<int> burnTickTimes = new List<int>();
    public List<int> poisonTickTimes = new List<int>();
    public List<int> radTickTimes = new List<int>();

    public int burnDamage = 5;
    public int poisonDamage = 10;
    public int lightningDamage = 50;
    public int radDamage = 2;

    void Start()
    {
        enemy = GetComponent<IEnemy>();
    }

    public void ApplyBurn(int ticks)
    {
        //damage will be passed in later and into the coroutine
        if (burnTickTimes.Count <= 0)
        {
            burnTickTimes.Add(ticks);
            StartCoroutine(Burn());
        }
        else
        {
            burnTickTimes.Add(ticks);
        }
    }

    IEnumerator Burn()
    {
        while(burnTickTimes.Count > 0)
        {
            for (int i = 0; i < burnTickTimes.Count; i++)
            {
                burnTickTimes[i]--;
            }
            enemy.TakeDmg(burnDamage);
            burnTickTimes.RemoveAll(num => num == 0);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ApplyPoison(int ticks)
    {
        //damage will be passed in later and into the coroutine
        if (poisonTickTimes.Count <= 0)
        {
            poisonTickTimes.Add(ticks);
            StartCoroutine(Poison());
        }
        else
        {
            poisonTickTimes.Add(ticks);
        }
    }

    IEnumerator Poison()
    {
        while (poisonTickTimes.Count > 0)
        {
            for (int i = 0; i < poisonTickTimes.Count; i++)
            {
                poisonTickTimes[i]--;
            }
            enemy.TakeDmg(poisonDamage);
            poisonTickTimes.RemoveAll(num => num == 0);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ApplyLightning()
    {
        //damage will be passed in later
        StartCoroutine(Lightning());
    }

    IEnumerator Lightning()
    {
        yield return new WaitForSeconds(1.5f);
        enemy.TakeDmg(lightningDamage);
    }

    public void ApplySmite()
    {
        StartCoroutine(Smite());
    }

    IEnumerator Smite()
    {
        yield return new WaitForSeconds(0.1f);
        enemy.Die();
    }

    public void ApplySlow()
    {
        StartCoroutine(Slow());
    }

    IEnumerator Slow()
    {
        yield return new WaitForSeconds(0.1f);
        //change the movespeed by saving and then decreasing before restoring it
    }

    public void ApplyRadioactive(int ticks)
    {
        //damage will be passed in later and into the coroutine
        if (radTickTimes.Count <= 0)
        {
            radTickTimes.Add(ticks);
            StartCoroutine(Radioactive());
        }
        else
        {
            radTickTimes.Add(ticks);
        }
    }

    IEnumerator Radioactive()
    {
        while (radTickTimes.Count > 0)
        {
            for (int i = 0; i < radTickTimes.Count; i++)
            {
                radTickTimes[i]--;
            }

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<IEnemy>() != null && hitCollider != this.GetComponent<Collider>())
                {
                    hitCollider.GetComponent<IEnemy>().TakeDmg(radDamage);
                }
            }

            radTickTimes.RemoveAll(num => num == 0);
            yield return new WaitForSeconds(0.5f);
        }    
    }
}
