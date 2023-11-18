using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemySpeed;
    [SerializeField] private Stats health;
    [SerializeField] private Element elementType;

    private Stack<Node> enemyPath;
    private Vector3 destination;
    private SpriteRenderer spriteRenderer;
    private int invulnerability = 2;
    private List<Debuff> debuffs = new List<Debuff>();
    private List<Debuff> debuffsToRemove = new List<Debuff>();
    private List<Debuff> newDebuffs = new List<Debuff>();

    // Properties
    public Point GridPosition { get; set; }
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
    }

    public void Spawn(int health)
    {
        // Setting spawn position to where the entrance is
        transform.position = LevelManager.Instance.Entrance.transform.position;
        this.health.Bar.ResetBar();
        this.health.MaxVal = health;
        this.health.CurrentVal = this.health.MaxVal;

        SetPath(LevelManager.Instance.EnemyPath);
    }

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

    // Reset enemy stats on exit and make it re-usable
    private void ResetEnemy()
    {
        IsActive = true;
        GridPosition = LevelManager.Instance.DoorOut;
        GameManager.Instance.Pool.ObjectReset(gameObject);

        // Enemy removes itself after exiting
        GameManager.Instance.EnemyRemove(this);
    }

    public void TakeDamage(float damage, Element damageSource)
    {
        if (IsActive)
        {
            if(damageSource == elementType)
            {
                // To make sure not to deal too much damage
                damage /= invulnerability;
                invulnerability++;
            }

            // Reduce health
            health.CurrentVal -= damage;

            if (health.CurrentVal <= 0 )
            {
                // Get some money for killing an enemy
                GameManager.Instance.Currency += 2;

                GameManager.Instance.Pool.ObjectReset(gameObject);
                GameManager.Instance.EnemyRemove(this);

     
            }
        }
    }

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

    public void RemoveDebuff(Debuff debuff)
    {
        // Add debuffs needed to be removed to the list
        debuffsToRemove.Add(debuff);
    }

    // Add and remove debuffs accordingally, then update.
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
