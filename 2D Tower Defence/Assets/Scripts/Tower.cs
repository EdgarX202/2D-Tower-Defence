using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Enemy target;

    private Queue<Enemy> enemies = new Queue<Enemy>();

    private void Start()
    {
        // Reference to sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Attack();

        // Check which target is in range or null
        Debug.Log(target);
    }

    public void Select()
    {
        // Enable-Disable spriteRenderer
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void Attack()
    {
        if (target == null && enemies.Count > 0)
        {
            // Remove the enemy that left towers range from queue
            target = enemies.Dequeue();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            // Add an enemy to the queue
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
