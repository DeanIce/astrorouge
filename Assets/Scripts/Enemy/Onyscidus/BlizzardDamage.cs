using UnityEngine;
using Managers;

public class BlizzardDamage : MonoBehaviour
{
    public int damage = 1;
    private float damageTimer = 0.5f;
    private float entryTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update() {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.GetComponent<PlayerDefault>() != null){
            entryTime = Time.time;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.root.gameObject.GetComponent<PlayerDefault>() != null && Time.time - entryTime > damageTimer){
            entryTime = Time.time;
            DamagePlayer(other);
        }
    }

    private void OnTriggerExit(Collider other) {

    }

    private void DamagePlayer(Collider other) {
        other.transform.root.gameObject.GetComponent<PlayerDefault>()?.TakeDmg(damage);

        // chance for slow effect
            if (Random.value < 0.3) {
                var slow = new SlowEffect();
                slow.ApplyEffect(other.transform.root.gameObject);
            }
    }
}
