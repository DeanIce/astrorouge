using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gravity;

public class StatusEffectManager : MonoBehaviour
{
    private IDamageable damageable;
    private List<int> burnTickTimes = new List<int>();
    private List<int> poisonTickTimes = new List<int>();
    private List<int> radTickTimes = new List<int>();
    private List<int> slowTickTimes = new List<int>();
    private List<int> stunTickTimes = new List<int>();

    [SerializeField] GameObject BurnVFX;
    [SerializeField] GameObject PosionVFX;
    [SerializeField] GameObject LightningVFX;
    [SerializeField] GameObject RadiationVFX;
    [SerializeField] GameObject SmiteVFX;
    [SerializeField] GameObject MartydomVFX;
    [SerializeField] GameObject IgniteVFX;
    [SerializeField] GameObject StunVFX;
    [SerializeField] GameObject SlowVFX;

    public int burnDamage = 5;
    public int poisonDamage = 10;
    public int lightningDamage = 50;
    public int radDamage = 2;
    public float smiteDamage = float.MaxValue;

    public bool martyrdom = false;
    public bool ignite = false;


    void Start()
    {
        damageable = GetComponent<IDamageable>();

        LightningVFX.GetComponent<ParticleSystem>().Stop(true);
        SmiteVFX.GetComponent<ParticleSystem>().Stop(true);
        MartydomVFX.GetComponent<ParticleSystem>().Stop(true);

        BurnVFX.SetActive(false);
        PosionVFX.SetActive(false);
        RadiationVFX.SetActive(false);
        IgniteVFX.SetActive(false);
        StunVFX.SetActive(false);
        SlowVFX.SetActive(false);
    }

    private void FixedUpdate()
    {
        straightenEffects(SmiteVFX);
        straightenEffects(SlowVFX);
    }

    private void straightenEffects(GameObject effect)
    {
        var sumForce = GravityManager.GetGravity(effect.transform.position, out var upAxis);
        //effect.GetComponent<Rigidbody>().AddForce(sumForce * Time.deltaTime);
        Debug.DrawLine(effect.transform.position, sumForce, Color.blue);

        // Upright?
        effect.GetComponent<Rigidbody>().MoveRotation(Quaternion.FromToRotation(effect.transform.up, upAxis) * effect.transform.rotation);
    }

    public void DeathEffects()
    {
        StartCoroutine(Martyrdom());
        StartCoroutine(Ignite());
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
        BurnVFX.SetActive(true);
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
        BurnVFX.SetActive(false);
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
        PosionVFX.SetActive(true);
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
        PosionVFX.SetActive(false);
    }

    public void ApplyLightning()
    {
        //damage will be passed in later
        StartCoroutine(Lightning());
    }

    IEnumerator Lightning()
    {
        yield return new WaitForSeconds(1.5f);
        LightningVFX.GetComponent<ParticleSystem>().Play(true);
        damageable.TakeDmg(lightningDamage);
    }

    public void ApplySmite()
    {
        StartCoroutine(Smite());
    }

    IEnumerator Smite()
    {
        yield return new WaitForSeconds(0.1f);
        SmiteVFX.GetComponent<ParticleSystem>().Play(true);
        damageable.TakeDmg(smiteDamage);
    }

    public void ApplySlow(int ticks)
    {
        if (slowTickTimes.Count <= 0)
        {
            slowTickTimes.Add(ticks);
            StartCoroutine(Slow());
        }
        else
        {
            slowTickTimes.Add(ticks);
        }
    }

    IEnumerator Slow()
    {
        SlowVFX.SetActive(true);
        float initSpeed = 1.0f;

        if(GetComponent<IEnemy>() != null)
        {
            initSpeed = GetComponent<IEnemy>().getSpeed();
            GetComponent<IEnemy>().setSpeed(initSpeed * 0.2f);
        }

        while (slowTickTimes.Count > 0)
        {
            for (int i = 0; i < slowTickTimes.Count; i++)
            {
                slowTickTimes[i]--;
            }
            slowTickTimes.RemoveAll(num => num == 0);
            yield return new WaitForSeconds(1f);
        }

        if (GetComponent<IEnemy>() != null)
        {
            GetComponent<IEnemy>().setSpeed(initSpeed);
        }
        SlowVFX.SetActive(false);
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
        RadiationVFX.SetActive(true);
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
        RadiationVFX.SetActive(false);
    }

    public void ApplyStun(int ticks)
    {
        if (stunTickTimes.Count <= 0)
        {
            stunTickTimes.Add(ticks);
            StartCoroutine(Stun());
        }
        else
        {
            stunTickTimes.Add(ticks);
        }
    }

    IEnumerator Stun()
    {
        StunVFX.SetActive(true);
        float initSpeed = 1.0f;

        if (GetComponent<IEnemy>() != null)
        {
            initSpeed = GetComponent<IEnemy>().getSpeed();
            GetComponent<IEnemy>().setSpeed(0.0f);
        }

        while (stunTickTimes.Count > 0)
        {
            for (int i = 0; i < stunTickTimes.Count; i++)
            {
                stunTickTimes[i]--;
            }
            stunTickTimes.RemoveAll(num => num == 0);
            yield return new WaitForSeconds(1.0f);
        }

        if (GetComponent<IEnemy>() != null)
        {
            GetComponent<IEnemy>().setSpeed(initSpeed);
        }
        StunVFX.SetActive(false);
    }

    public void ApplyMartyrdom()
    {
        martyrdom = true;
    }

    IEnumerator Martyrdom()
    {
        if (martyrdom)
        {
            MartydomVFX.GetComponent<ParticleSystem>().Play(true);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<IEnemy>() != null && hitCollider != this.GetComponent<Collider>())
                {
                    hitCollider.GetComponent<IEnemy>().TakeDmg(lightningDamage);
                }
            }
        }

        yield return null;
    }

    public void ApplyIgnite()
    {
        ignite = true;
    }

    IEnumerator Ignite()
    {
        if (ignite)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<IEnemy>() != null && hitCollider != this.GetComponent<Collider>())
                {
                    hitCollider.GetComponent<StatusEffectManager>().ApplyBurn(6);
                }
            }
            IgniteVFX.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            IgniteVFX.SetActive(false);
        }
        else
        {
            yield return null;
        }
    }
}
