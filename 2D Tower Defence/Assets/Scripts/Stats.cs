using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    // Serialised Fields
    [SerializeField] private HBar bar;
    [SerializeField] private float maxVal;
    [SerializeField] private float currentVal;

    // Properties
    public float CurrentVal
    {
        get { return currentVal; }
        set 
        {
            this.currentVal = Mathf.Clamp(value,0,MaxVal);
            bar.Value = currentVal;
        }
    }
    public float MaxVal
    {
        get { return maxVal; }
        set
        {
            this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }
    public HBar Bar
    {
        get { return bar; }
    }

    // Initialise max and current values
    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
}
