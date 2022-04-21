using UnityEngine;

public class IngenalvusAttacks : MonoBehaviour
{
    public GameObject fireParticles;
    private Animator animator;
    private ParticleSystem particles;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        particles = fireParticles.GetComponent<ParticleSystem>();
        particles.Stop();
        // particles.emission.rateOverDistanceMultiplier = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void BreathFireStart()
    {
        particles.Play();
    }

    public void BreathFireStop()
    {
        particles.Stop();
    }
}