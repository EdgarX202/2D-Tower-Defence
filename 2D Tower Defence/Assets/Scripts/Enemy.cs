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
    public bool IsAlive
    {
        // Return true is health is more than 0, otherwise the enemy is dead
        get { return health.CurrentVal > 0; }
    }
    public Element ElementType { get { return elementType; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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
                damage = damage / invulnerability;
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
            // If its a new debuff, add
            newDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);
    }

    public void HandleDebuff()
    {
        if(newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);

            newDebuffs.Clear();
        }

        foreach(Debuff debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff);
        }

        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
}
