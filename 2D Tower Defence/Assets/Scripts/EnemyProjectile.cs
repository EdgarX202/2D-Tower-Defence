using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Private
    private Enemy parent;
    private Tower target;

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    // Initialise target, parent and element type
    public void Initialize(Enemy parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }

    // Move projectile towards the enemy
    private void MoveToTarget()
    {
        // If target is not null and active
        if (target != null && target.IsActive)
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
        }
        else if (!target.IsActive)
        {
            // Reset projectile - send it back to object pool
            GameManager.Instance.Pool.ObjectReset(gameObject);
        }
    }

    

    // Do damage and apply debuff if the enemy is hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tower")
        {
            if (target.gameObject == collision.gameObject)
            {
                Debug.Log("Hit the tower");
                // Do damage
               // target.TakeDamage(parent.Damage, elementType);
                // Reset projectile
                GameManager.Instance.Pool.ObjectReset(gameObject);
            }
        }
    }
}
