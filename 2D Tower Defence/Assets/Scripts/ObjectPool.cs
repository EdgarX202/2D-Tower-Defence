using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPoolPrefabs;

    private List<GameObject> poolObject = new List<GameObject>();

    public GameObject getObject(string type)
    {
        foreach (GameObject obj in poolObject)
        {
            if(obj.name == type && !obj.activeInHierarchy)
            {
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

    public void ObjectReset(GameObject gameObj)
    {
        gameObj.SetActive(false);
    }
}
