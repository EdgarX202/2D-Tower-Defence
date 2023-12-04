using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Serialised Fields
    [SerializeField] private GameObject[] objectPoolPrefabs;

    // Lists
    private List<GameObject> poolObject = new List<GameObject>();

    // Activate/Instantiate pool objects
    public GameObject getObject(string type)
    {
        // Check if object is in the pool and not active
        foreach (GameObject obj in poolObject)
        {
            if(obj.name == type && !obj.activeInHierarchy)
            {
                // Make the object active
                obj.SetActive(true);
                return obj;
            }
        }

        // Run through the object pool
        for(int i = 0; i < objectPoolPrefabs.Length; i++)
        {
            // If prefab exists
            if (objectPoolPrefabs[i].name == type)
            {
                // Instantiate it
                GameObject newObject = Instantiate(objectPoolPrefabs[i]);
                poolObject.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }

        return null;
    }

    // Make the object inactive
    public void ObjectReset(GameObject gameObj)
    {
        gameObj.SetActive(false);
    }
}
