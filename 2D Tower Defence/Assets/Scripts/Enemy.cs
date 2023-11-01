using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float enemySpeed;

    private GameObject targetTile;

    //private int reward;
    //private int damage;

    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        targetTile = MapGenerator.startTile;
    }

    // Subtract enemy health
    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;

        if (enemyHealth <= 0)
        {
            Die();
        }
    }


    // Destroy the enemy when it dies
    private void Die()
    {
        Enemies.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    // Enemy movement to targetTile (players base)
    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, enemySpeed * Time.deltaTime);
    }

    private void CheckPosition()
    {
        // Check if target tile exist is not the end tile
        if(targetTile != null && targetTile != MapGenerator.endTile)
        {
            // Calculate distance between enemys and target tiles position
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if(distance < 0.001f)
            {
                // Set the target tile to the next one
                int currentIndex = MapGenerator.pathTiles.IndexOf(targetTile);

                targetTile = MapGenerator.pathTiles[currentIndex + 1];
            }
        }
    }

    private void Update()
    {
        CheckPosition();
        MoveEnemy();
        TakeDamage(0);
    }
}
