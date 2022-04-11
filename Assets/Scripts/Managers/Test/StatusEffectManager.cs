using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public GameObject BurnVFX;
    public GameObject PosionVFX;
    public GameObject LightningVFX;
    public GameObject RadiationVFX;
    public GameObject SmiteVFX;
    public GameObject MartydomVFX;
    public GameObject IgniteVFX;
    public GameObject StunVFX;
    public GameObject SlowVFX;

    public static int BurnDamage() => 5 * (Managers.LevelSelect.Instance.requestedLevel + 1);
    public static int PoisonDamage() => 10 * (Managers.LevelSelect.Instance.requestedLevel + 1);
    public static int LightningDamage() => 25 * (Managers.LevelSelect.Instance.requestedLevel + 1);
    public static int RadDamage() => 2 * (Managers.LevelSelect.Instance.requestedLevel + 1);
    public float smiteDamage = float.MaxValue;

    public bool martyrdom;
    public bool ignite;
    private readonly List<int> burnTickTimes = new();
    private readonly List<int> poisonTickTimes = new();
    private readonly List<int> radTickTimes = new();
    private readonly List<int> slowTickTimes = new();
    private readonly List<int> stunTickTimes = new();
    private IDamageable damageable;


    private void Start()
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

    public void DeathEffects()
    {
        StartCoroutine(Martyrdom());
        StartCoroutine(Ignite());
    }

    public void ApplyBurn(int ticks)
    {
        if (burnTickTimes.Count <= 0)
        {
            burnTickTimes.Add(ticks);
            StartCoroutine(Burn());
        }
        else
            burnTickTimes.Add(ticks);
    }

    private IEnumerator Burn()
    {
        if (BurnVFX)
        {
            BurnVFX.SetActive(true);
            while (burnTickTimes.Count > 0)
            {
                for (var i = 0; i < burnTickTimes.Count; i++)
                {
                    burnTickTimes[i]--;
                }

                damageable.TakeDmg(BurnDamage());
                burnTickTimes.RemoveAll(num => num == 0);
                yield return new WaitForSeconds(0.5f);
            }

            BurnVFX.SetActive(false);
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
            poisonTickTimes.Add(ticks);
    }

    private IEnumerator Poison()
    {
        if (PosionVFX)
        {
            PosionVFX.SetActive(true);
            while (poisonTickTimes.Count > 0)
            {
                for (var i = 0; i < poisonTickTimes.Count; i++)
                {
                    poisonTickTimes[i]--;
                }

                damageable.TakeDmg(PoisonDamage());
                poisonTickTimes.RemoveAll(num => num == 0);
                yield return new WaitForSeconds(1f);
            }

            PosionVFX.SetActive(false);
        }
    }

    public void ApplyLightning()
    {
        //damage will be passed in later
        StartCoroutine(Lightning());
    }

    private IEnumerator Lightning()
    {
        yield return new WaitForSeconds(1.5f);
        LightningVFX.GetComponent<ParticleSystem>().Play(true);
        damageable.TakeDmg(LightningDamage());
    }

    public void ApplySmite()
    {
        StartCoroutine(Smite());
    }

    private IEnumerator Smite()
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
            slowTickTimes.Add(ticks);
    }

    private IEnumerator Slow()
    {
        if (SlowVFX)
        {
            SlowVFX.SetActive(true);
            var initSpeed = 1.0f;

            if (GetComponent<IEnemy>() != null)
            {
                initSpeed = GetComponent<IEnemy>().getSpeed();
                GetComponent<IEnemy>().setSpeed(initSpeed * 0.2f);
            }

            while (slowTickTimes.Count > 0)
            {
                for (var i = 0; i < slowTickTimes.Count; i++)
                {
                    slowTickTimes[i]--;
                }

                slowTickTimes.RemoveAll(num => num == 0);
                yield return new WaitForSeconds(1f);
            }

            if (GetComponent<IEnemy>() != null) GetComponent<IEnemy>().setSpeed(initSpeed);
            SlowVFX.SetActive(false);
        }
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
            radTickTimes.Add(ticks);
    }

    private IEnumerator Radioactive()
    {
        if (RadiationVFX)
        {
            RadiationVFX.SetActive(true);
            while (radTickTimes.Count > 0)
            {
                for (var i = 0; i < radTickTimes.Count; i++)
                {
                    radTickTimes[i]--;
                }

                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.GetComponent<IEnemy>() != null && hitCollider != GetComponent<Collider>())
                        hitCollider.GetComponent<IEnemy>().TakeDmg(RadDamage());
                }

                radTickTimes.RemoveAll(num => num == 0);
                yield return new WaitForSeconds(0.5f);
            }

            RadiationVFX.SetActive(false);
        }
    }

    public void ApplyStun(int ticks)
    {
        if (stunTickTimes.Count <= 0)
        {
            stunTickTimes.Add(ticks);
            StartCoroutine(Stun());
        }
        else
            stunTickTimes.Add(ticks);
    }

    private IEnumerator Stun()
    {
        if (StunVFX)
        {
            StunVFX.SetActive(true);
            var initSpeed = 1.0f;

            if (GetComponent<IEnemy>() != null)
            {
                initSpeed = GetComponent<IEnemy>().getSpeed();
                GetComponent<IEnemy>().setSpeed(0.0f);
            }

            while (stunTickTimes.Count > 0)
            {
                for (var i = 0; i < stunTickTimes.Count; i++)
                {
                    stunTickTimes[i]--;
                }

                stunTickTimes.RemoveAll(num => num == 0);
                yield return new WaitForSeconds(1.0f);
            }

            if (GetComponent<IEnemy>() != null) GetComponent<IEnemy>().setSpeed(initSpeed);
            StunVFX.SetActive(false);
        }
    }

    public void ApplyMartyrdom()
    {
        martyrdom = true;
    }

    private IEnumerator Martyrdom()
    {
        if (martyrdom && MartydomVFX)
        {
            MartydomVFX.GetComponent<ParticleSystem>().Play(true);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<IEnemy>() != null && hitCollider != GetComponent<Collider>())
                    hitCollider.GetComponent<IEnemy>().TakeDmg(LightningDamage());
            }
        }

        yield return null;
    }

    public void ApplyIgnite()
    {
        ignite = true;
    }

    private IEnumerator Ignite()
    {
        if (ignite && IgniteVFX)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<IEnemy>() != null && hitCollider != GetComponent<Collider>())
                    hitCollider.GetComponent<StatusEffectManager>().ApplyBurn(BurnDamage());
            }

            IgniteVFX.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            IgniteVFX.SetActive(false);
        }
        else
            yield return null;
    }
}