using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float timeBetweenShots;

    private float nextTimeToShoot;

    public GameObject currentTarget;

    private void Start()
    {
        /*
         * Time.time = total amount of time since the game started
         * Time.deltaTime = the amount of timee took to render last frame
         */
        nextTimeToShoot = Time.time;
    }

    // Update which is the nearest enemy for the tower to target
    private void UpdateNearestEnemy()
    {
        GameObject currentNearestEnemy = null;
        float distance = Mathf.Infinity;

        // Calculate distance between tower and current enemy
        foreach (GameObject enemy in Enemies.enemies)
        {
            if(enemy != null)
            {
                float _distance = (transform.position - enemy.transform.position).magnitude;

                if (_distance < distance)
                {
                    distance = _distance;
                    currentNearestEnemy = enemy;
                }
            }
        }
        // If enemy distance is in range, make it the current target
        if (distance <= range)
        {
            currentTarget = currentNearestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }

    protected virtual void Shoot()
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
