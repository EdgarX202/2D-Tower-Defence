using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Enemy target;

    private Queue<Enemy> enemies = new Queue<Enemy>();

    private bool canAttack = true;
    private float attackTime;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float cooldown;
    [SerializeField] private string projectileType;

    // Properties
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    public Enemy Target
    {
        get { return target; }
    }

    private void Start()
    {
        // Reference to sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Attack();

        // Check which target is in range or null
        //Debug.Log(target);
    }

    public void Select()
    {
        // Enable-Disable spriteRenderer
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void Attack()
    {
        if(!canAttack)
        {
            attackTime += Time.deltaTime;

            // When attack time is larger than cooldown, a tower can shoot again, set the time back to 0
            if(attackTime >= cooldown)
            {
                canAttack = true;
                attackTime = 0;
            }
        }

        if (target == null && enemies.Count > 0)
        {
            // Remove the enemy that left towers range from queue
            target = enemies.Dequeue();
        }

        // If the target is in range and active, shoot once
        if(target != null && target.IsActive)
        {
            if(canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.getObject(projectileType).GetComponent<Projectile>();

        // Spawn in the middle of the tower
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            // Add an enemy to the queue when in range
            enemies.Enqueue(collision.GetComponent<Enemy>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            target = null;
        }
    }
}
