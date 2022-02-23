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
        GameObject root = target.transform.root.gameObject;

        root.GetComponent<IEnemy>()?.TakeDmg(damage);
        root.GetComponent<IProjectile>()?.TakeDmg(damage);
        root.GetComponent<IPlayer>()?.TakeDmg(damage);

        effects.ForEach(effect => effect.ApplyEffect(root));
    }

    public abstract void Die();
    public virtual Vector3 Displacement(float deltaTime) => deltaTime * velocity;
    public virtual void TakeDmg(float incDamage) => currHealth -= incDamage;
}