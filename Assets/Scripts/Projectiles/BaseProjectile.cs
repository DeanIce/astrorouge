using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour, IProjectile
{
    // Dynamic values
    [SerializeField] protected Vector3 velocity;
    [SerializeField] protected float timeLeft;
    [SerializeField] protected float currHealth = 1;
    [SerializeField] protected float damage;

    protected List<IEffect> effects = new();
    protected LayerMask collisionLayer;

    public void AttachEffect(IEffect effect) => effects.Add(effect);    
    protected void CollisionResponse(GameObject target)
    {
        target.GetComponent<IEnemy>()?.TakeDmg(damage);
        target.GetComponent<IProjectile>()?.TakeDmg(damage);
        target.GetComponent<IPlayer>()?.TakeDmg(damage);

        effects.ForEach(effect => effect.ApplyEffect(target));
    }

    public abstract void Die();
    public virtual Vector3 Displacement(float deltaTime) => deltaTime * velocity;
    public virtual void TakeDmg(float incDamage) => currHealth -= incDamage;
}