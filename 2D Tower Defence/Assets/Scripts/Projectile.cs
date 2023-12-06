using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Private
    private Enemy target;
    private Tower parent;
    private Element elementType;

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    // Initialise target, parent and element type
    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
        this.elementType = parent.ElementType;
    }

    // Move projectile towards the enemy
    private void MoveToTarget()
    {
        // If target is not null and active
        if(target != null && target.IsActive)
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

    // Apply debuff to the target
    private void ApplyDebuff()
    {
        // If projectile element type is different from target element type
        if(target.ElementType != elementType)
        {
            // Roll a random number
            float roll = Random.Range(0, 100);

            // If in range of the proc, apply debuff
            if(roll <= parent.Proc)
            {
                // Apply debuff to the target - parent is the tower that projectile comes from
                target.AddDebuff(parent.GetDebuff());
            }
        }
    }

    // Do damage and apply debuff if the enemy is hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.tag == "Enemy")
            {
                if (target.gameObject == collision.gameObject)
                {
                    // Do damage
                    target.TakeDamage(parent.Damage, elementType);
                    // Reset projectile
                    GameManager.Instance.Pool.ObjectReset(gameObject);
                    // Apply debuff
                    ApplyDebuff();
                }
            }
    }
}
