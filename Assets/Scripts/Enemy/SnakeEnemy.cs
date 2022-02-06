using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : RangedEnemy
{
    Animator animator;
    [SerializeField] GameObject projectile;

    public override void Start()
    {
        Dying = false;
        animator = GetComponentInChildren<Animator>();
        base.Start();
    }

    public override IEnumerator Attack()
    {
        RaycastHit[] hits;
        //rend.enabled = true;
        Attacking = true;
        animator.SetBool("attack3", true);
        yield return new WaitForSeconds(2f);
        GameObject ball = Instantiate(projectile);
        ball.transform.position = transform.position + transform.up * 3;
        ball.GetComponent<Rigidbody>().MoveRotation(Quaternion.FromToRotation(ball.transform.forward, transform.forward) * transform.rotation);
        //rend.enabled = false;
        Attacking = false;
        animator.SetBool("attack3", false);
    }

    public override void Die()
    {
        if (!Dying) StartCoroutine(DieCo());
    }

    private IEnumerator DieCo()
    {
        Dying = true;
        animator.SetBool("attack3", false);
        animator.SetBool("death", true);
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }
}
