using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


// ENUM for tower effects
public enum Element
{
    ELECTRIC, POISON, FLAME, ICE, NONE
}

public abstract class Tower : MonoBehaviour
{
    // Private
    private SpriteRenderer spriteRenderer;

    private Enemy target;

    private Queue<Enemy> enemies = new Queue<Enemy>();

    private bool canAttack = true;
    private float attackTime;

    // Serialized fields
    [SerializeField] private string projectileType;
    [SerializeField] private int damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float cooldown;
    [SerializeField] private float debuffDuration;
    [SerializeField] private float proc; // Programmed random occurance - random event triggered by specific action/condition

    // Properties
    public float ProjectileSpeed { get { return projectileSpeed; } }
    public Enemy Target { get { return target; } }
    public int Damage { get { return damage; } }
    public int Price { get; set; }
    public Element ElementType { get; protected set; }
    public float Proc 
    { 
        get { return proc; } 
        set { this.proc = value; }
    }
    public float DebuffDuration
    {
        get { return debuffDuration; }
        set { this.debuffDuration = value; }
    }

    private void Start()
    {
        SetRenderer();
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

    public void SetRenderer()
    {
        // Reference to sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        else if(enemies.Count > 0)
        {
            target = enemies.Dequeue();
        }
        //if(target != null && target.IsAlive)
        //{
        //    target = null;
        //}
    }

    private void Shoot()
    {
        // Get the projectile from object pool
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

    // Return debuff of a specific type
    public abstract Debuff GetDebuff();

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            target = null;
        }
    }
}
