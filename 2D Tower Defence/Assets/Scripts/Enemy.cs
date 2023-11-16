using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemySpeed;

    private Stack<Node> enemyPath;
    private Vector3 destination;

    // Properties
    public Point GridPosition { get; set; }
    public bool IsActive { get; set; }

    private void Start()
    {
        IsActive = true;
    }
    private void Update()
    {
        EnemyMove();
    }

    public void Spawn()
    {
        // Setting spawn position to where the entrance is
        transform.position = LevelManager.Instance.Entrance.transform.position;

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
}
