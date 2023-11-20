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
    private int level; // Towers current level

    // Serialized fields
    [SerializeField] private string projectileType;
    [SerializeField] private int damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float cooldown;
    [SerializeField] private float debuffDuration;
    [SerializeField] private float proc; // Programmed random occurance - random event triggered by specific action/condition

    // Properties
    public float ProjectileSpeed { get { return projectileSpeed; } }
    public TowerUpgrade[] Upgrades { get; protected set; }
    public Enemy Target { get { return target; } }
    public int Damage { get { return damage; } }
    public int Price { get; set; }
    public Element ElementType { get; protected set; }
    public int Level { get; protected set; }
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
    public TowerUpgrade NextUpgrade
    {
        get
        {
            if(Upgrades.Length > Level-1)
            {
                // Return upgrade for the next level
                return Upgrades[Level-1];
            }
            return null;
        }
    }

    private void Awake()
    {
        Level = 1;
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

        GameManager.Instance.UpdateUpgradeTooltip();
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

        if (target == null && enemies.Count > 0 && enemies.Peek().IsActive)
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

        if (target != null && target.IsAlive || target != null && target.IsActive)
        {
            target = null;
        }
    }

    // Show current stats + what will be added with the next upgrade
    public virtual string GetStats()
    {
        // If next upgrade is available
        if(NextUpgrade != null)
        {
            // Return new upgrade stats
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#00ff00ff> +{4}</color> \nProc: {2}% <color=#00ff00ff> +{5}%</color> \nDebuff: {3}sec <color=#00ff00ff> +{6}</color>", Level, damage, proc, debuffDuration, NextUpgrade.Damage, NextUpgrade.Proc, NextUpgrade.DebuffDuration);
        }

        // Return the current upgrade
        return string.Format("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff: {3}s", Level, damage, proc, debuffDuration);
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

    public virtual void Upgrade()
    {
        // Reduce players money after buying an upgrade
        GameManager.Instance.Currency -= NextUpgrade.Price;
        // To update how expensive the tower is now
        Price += NextUpgrade.Price;
        // Increase tower stats
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.DebuffDuration;
        this.debuffDuration += NextUpgrade.DebuffDuration;
        // Add level
        Level++;
        // Update tooltip for the next upgrade
        GameManager.Instance.UpdateUpgradeTooltip();
    }
}
