using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private Tower parent;
    private Element elementType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
        this.elementType = parent.ElementType;
    }

    private void MoveToTarget()
    {
        if(target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
        }
        else if (!target.IsActive)
        {
            // Reset projectile - send it back to object pool
            GameManager.Instance.Pool.ObjectReset(gameObject);
        }
    }

    private void ApplyDebuff()
    {
        // If projectile element type is different from target element type
        if(target.ElementType != elementType)
        {
            float roll = Random.Range(0, 100);

            if(roll <= parent.Proc)
            {
                // Add debuff to the target - parent is the tower that projectile comes from
                target.AddDebuff(parent.GetDebuff());
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if(target.gameObject == collision.gameObject)
            {
                target.TakeDamage(parent.Damage, elementType);

                GameManager.Instance.Pool.ObjectReset(gameObject);

                ApplyDebuff();
            }
        }

    }
}
