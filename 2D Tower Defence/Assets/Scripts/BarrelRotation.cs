using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    // Reference to tower class
    public Tower tower;

    public Transform pivot;
    public Transform barrel;

    private void Update()
    {
        if (tower != null)
        {
            if (tower.currentTarget != null)
            {
                // Relative position of the targets position - barrels pivot position
                Vector2 relative = tower.currentTarget.transform.position - pivot.position;

                // Calculate the angle between barrels and desired rotation
                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

                // Set new rotation
                Vector3 newRotation = new Vector3 (0, 0, angle);
                pivot.localRotation = Quaternion.Euler (newRotation);
            }
        }
    }
}
