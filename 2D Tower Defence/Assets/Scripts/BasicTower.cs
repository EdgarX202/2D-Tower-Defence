using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    /*
     * This class inherits from Tower class
     */

    public Transform pivot;
    public Transform barrel;

    public GameObject bullet;

    protected override void Shoot()
    {
        base.Shoot();

        GameObject newBullet = Instantiate(bullet, barrel.position, pivot.rotation);
    }
}
