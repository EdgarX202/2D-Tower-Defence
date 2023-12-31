using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Serialised fields
    [SerializeField] private string projectileType;
    [SerializeField] private float enemySpeed;
    [SerializeField] private Stats health;
    [SerializeField] private int damage;
    [SerializeField] private Element elementType;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float cooldown;

    // Private
    private int invulnerability = 2;
    private Vector3 destination;
    private Tower target;
    private Stack<Node> enemyPath;
    private SpriteRenderer spriteRenderer;
    private List<Debuff> debuffs = new List<Debuff>();
    private List<Debuff> debuffsToRemove = new List<Debuff>();
    private List<Debuff> newDebuffs = new List<Debuff>();
    private bool canAttack = true;
    private float attackTime = 0;
    private Queue<Tower> enemies = new Queue<Tower>();

    // Properties
    public Tower Target { get { return target; } }
    public Point GridPosition { get; set; }
    public float ProjectileSpeed { get { return projectileSpeed; } }
    public bool IsActive { get; set; }
    public float MaxSpeed { get; set; }
    public bool IsAlive
    {
        // Return true is health is more than 0, otherwise the enemy is dead
        get { return health.CurrentVal > 0; }
    }
    public Element ElementType { get { return elementType; } }
    public float EnemySpeed
    {
        get { return enemySpeed; }
        set { this.enemySpeed = value; }
    }
    public int Damage { get { return damage; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxSpeed = enemySpeed;
        health.Initialize();  
    }

    private void Start()
    {
        IsActive = true;
    }

    private void Update()
    {
        HandleDebuff();
        EnemyMove();
        Attack();
    }

    // Spawn enemy settings
    public void Spawn(int health)
    {
        // Remove all exisitng debuffs
        debuffs.Clear();

        // Setting spawn position to where the entrance is
        transform.position = LevelManager.Instance.Entrance.transform.position;
        // Health settings
        this.health.Bar.ResetBar();
        this.health.MaxVal = health;
        this.health.CurrentVal = this.health.MaxVal;

        SetPath(LevelManager.Instance.EnemyPath);
    }

    // Move enemy from A to B
    private void EnemyMove()
    {
        if(IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, enemySpeed * Time.deltaTime);

            // Each tile is a destination to where enemy will move next -- not the final goal
            if (transform.position == destination)
            {
                if (enemyPath != null && enemyPath.Count > 0)
                {
                    // Look at the grid position and store it
                    GridPosition = enemyPath.Peek().GridPosition;
                    // Remove the tile from the enemy path
                    destination = enemyPath.Pop().WorldPos;
                }
            }
        }
    }

    // Setting enemy path
    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.enemyPath = newPath;

            // Look at the grid position and store it
            GridPosition = enemyPath.Peek().GridPosition;
            // Remove the tile from the enemy path
            destination = enemyPath.Pop().WorldPos;
        }
    }

    // Reduce player lives when enemy collides with the target
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == "Exit")
        {
            ResetEnemy();

            GameManager.Instance.Lives--;
        }

        if(collision.tag == "Tile")
        {
            spriteRenderer.sortingOrder = collision.GetComponent<TileScript>().GridPosition.Y;
        }
    }

    // Reset enemy stats on exit and make it re-usable (object pooling)
    private void ResetEnemy()
    {
        // Clear all debuffs
        debuffs.Clear();

        // Set enemy speed back to normal
        enemySpeed = MaxSpeed;

        IsActive = true;
        GridPosition = LevelManager.Instance.DoorOut;
        // Remove and reset the enemy from the game
        GameManager.Instance.Pool.ObjectReset(gameObject);
        GameManager.Instance.EnemyRemove(this);
    }


    // Tower can shoot
    private void Attack()
    {
        if (!canAttack)
        {
            attackTime += Time.deltaTime;

            // When attack time is larger than cooldown, a tower can shoot again, set the time back to 0
            if (attackTime >= cooldown)
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
        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }

        if (target != null || target != null && target.IsActive)
        {
            target = null;
        }
    }

    // Shoot projectile
    private void Shoot()
    {
        // Get the projectile from object pool
        EnemyProjectile projectile = GameManager.Instance.Pool.getObject(projectileType).GetComponent<EnemyProjectile>();

        // Spawn in the middle of the tower
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    // Reduce enemy health
    public void TakeDamage(float damage, Element damageSource)
    {
        if (IsActive)
        {
            // The enemy can be resistant to certain tower types and take less damage
            if(damageSource == elementType)
            {
                // Deal half the original damage amount
                damage /= invulnerability;
                // Increase resistance
                invulnerability++;
            }

            // Reduce health
            health.CurrentVal -= damage;

            // If out of health, remove the enemy and give coins to the player
            if (health.CurrentVal <= 0 )
            {
                SoundManager.Instance.PlaysEffects("damage");

                // Player receives coins for killing an enemy
                GameManager.Instance.Currency += 5;

                GameManager.Instance.Pool.ObjectReset(gameObject);
                GameManager.Instance.EnemyRemove(this);

     
            }
        }
    }

    // Adding debuff to an enemy
    public void AddDebuff(Debuff debuff)
    {
        // Check the list of debuffs and everytime we find an element, call x
        // and check if x type is == debuff type
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            // If its a new debuff, add it to the list
            newDebuffs.Add(debuff);
        }
    }

    // Remove debuff from enemy once no longer needed
    public void RemoveDebuff(Debuff debuff)
    {
        // Add debuffs needed to be removed to the list
        debuffsToRemove.Add(debuff);
    }

    // Add, remove, update debuffs accordingally.
    public void HandleDebuff()
    {
        // If there are new debuffs
        if(newDebuffs.Count > 0)
        {
            // Add to debuffs list
            debuffs.AddRange(newDebuffs);

            // Clear the list 
            newDebuffs.Clear();
        }

        foreach(Debuff debuff in debuffsToRemove)
        {
            // Remove debuffs
            debuffs.Remove(debuff);
        }

        // Clear the list
        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs)
        {
            // Update every debuff
            debuff.Update();
        }
    }
}
