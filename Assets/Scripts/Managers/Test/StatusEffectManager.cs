using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private IEnemy enemy;
    public List<int> burnTickTimes = new List<int>();

    void Start()
    {
        enemy = GetComponent<IEnemy>();
    }

    public void ApplyBurn(int ticks)
    {
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
            enemy.TakeDmg(5);
            burnTickTimes.RemoveAll(num => num == 0);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
