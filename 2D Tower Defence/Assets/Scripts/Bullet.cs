using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed = 0.10f;

    private void Start()
    {
        // Incase the bullet doesnt hit the enemy destroy after 10s of its existence
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += transform.right * bulletSpeed;
    }
}
