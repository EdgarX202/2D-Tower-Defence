using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private float range;
    private float damage;
    private float timeBetweenShots;

    private float nextTimeToShoot;

    private GameObject currentTarget;

    private void Start()
    {
        nextTimeToShoot = Time.time;
    }

    private void UpdateNearestEnemy()
    {
        GameObject currentNearestEnemy = null;

        float distance = Mathf.Infinity;

        // Calculate distance between tower and current enemy
        foreach (GameObject enemy in Enemies.enemies)
        {
            float _distance = (transform.position - enemy.transform.position).magnitude;

            if(_distance < distance)
            {
                distance = _distance;
                currentNearestEnemy = enemy;
            }
        }

        if (distance <= range)
        {
            currentTarget = currentNearestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    private void Shoot()
    {   
       Enemy enemyScript = currentTarget.GetComponent<Enemy>();

       enemyScript.TakeDamage(damage);
    }

    private void Update()
    {
        UpdateNearestEnemy();
        
        if(Time.time >= nextTimeToShoot)
        {
            if(currentTarget != null)
            {
                Shoot();
                nextTimeToShoot += Time.time + timeBetweenShots;
            }
        }
    }
}
