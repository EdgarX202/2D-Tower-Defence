using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTile : MonoBehaviour
{
    /*
     * CALCULATE PATHFINDING TILE COSTS
     * 
     * G - cost/distance of the path travelled so far from the start node to current node
     * H - estimate of the remaining cost/distance to reach goal node from current node. Heuristic function, provides approximation of the cost
     * F - total cost/distance, sum of G and H
     * 
     * f(n) = g(n) + h(n)
     */

    // Serialised fields
    [SerializeField] private TextMeshProUGUI f;
    [SerializeField] private TextMeshProUGUI g;
    [SerializeField] private TextMeshProUGUI h;

    // Properties
    public TextMeshProUGUI F
    {
        get 
        {
            f.gameObject.SetActive(true);
            return f; 
        }
        set { this.f = value; }
    }
    public TextMeshProUGUI G
    {
        get 
        {
            g.gameObject.SetActive(true);
            return g; 
        }
        set { this.g = value; }
    }
    public TextMeshProUGUI H
    {
        get 
        {
            h.gameObject.SetActive(true);
            return h; 
        }
        set { this.h = value; }
    }
}
