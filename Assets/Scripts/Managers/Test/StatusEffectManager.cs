using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private IDamageable damageable;
    public List<int> burnTickTimes = new List<int>();
    public List<int> poisonTickTimes = new List<int>();
    public List<int> radTickTimes = new List<int>();

    public int burnDamage = 5;
    public int poisonDamage = 10;
    public int lightningDamage = 50;
    public int radDamage = 2;
    public float smiteDamage = float.MaxValue;
    public bool slowed = false;

    void Start()
    {
        damageable = GetComponent<IDamageable>();
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
            damageable.TakeDmg(burnDamage);
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
            damageable.TakeDmg(poisonDamage);
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
        damageable.TakeDmg(lightningDamage);
    }

    public void ApplySmite()
    {
        StartCoroutine(Smite());
    }

    IEnumerator Smite()
    {
        yield return new WaitForSeconds(0.1f);
        damageable.TakeDmg(smiteDamage);
    }

    public void ApplySlow()
    {
        if (!slowed) {
            slowed = true;
            StartCoroutine(Slow());
        }
    }

    IEnumerator Slow()
    {
        if(GetComponent<IEnemy>() != null)
        {
            GetComponent<IEnemy>().ChangeSpeed(0.2f);
        }

        yield return new WaitForSeconds(5.0f);

        if (GetComponent<IEnemy>() != null)
        {
            GetComponent<IEnemy>().ChangeSpeed(5.0f);
        }
        slowed = false;
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
