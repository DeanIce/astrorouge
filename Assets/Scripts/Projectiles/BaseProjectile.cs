using System.Collections.Generic;
using Enemy.Ingenalvus;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour, IProjectile
{
    // Dynamic values
    [SerializeField] protected Vector3 velocity;
    [SerializeField] protected float timeLeft;
    [SerializeField] protected float currHealth = 1;
    [SerializeField] protected float damage;
    protected LayerMask collisionLayer;

    protected List<IEffect> effects = new();

    public void AttachEffect(IEffect effect)
    {
        effects.Add(effect);
    }

    public void CopyEffects(IProjectile target)
    {
        foreach (IEffect effect in effects)
        {
            target.AttachEffect(effect);
        }
    }

    public abstract void Die();

    public virtual Vector3 Displacement(float deltaTime)
    {
        return deltaTime * velocity;
    }

    public virtual void TakeDmg(float incDamage)
    {
        currHealth -= incDamage;
    }

    public virtual void TakeDmg(float incDamage, int type)
    {
        currHealth -= incDamage;
    }

    protected void CollisionResponse(GameObject target)
    {
        GameObject root = target.transform.root.gameObject;

        root.GetComponent<IEnemy>()?.TakeDmg(damage);
        root.GetComponent<IProjectile>()?.TakeDmg(damage);
        root.GetComponent<IPlayer>()?.TakeDmg(damage);
        root.GetComponent<Ingenalvus>()?.TakeDmg(damage);
        root.GetComponent<IceBoss>()?.TakeDmg(damage);

        // target.GetComponent<IngenalvusCollider>()?.PassThroughDamage(damage);

        effects.ForEach(effect => effect.ApplyEffect(root));
    }
}