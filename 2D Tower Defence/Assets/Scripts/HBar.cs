using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HBar : MonoBehaviour
{
    private float fillAmount;

    [SerializeField] private float lerpSpeed;
    [SerializeField] private Image content;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color lowColor;

    // Properties
    public float MaxValue { get; set; }
    public float Value
    {
        set
        {
                fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }

        content.color = Color.Lerp(lowColor, fullColor, fillAmount);
    }

    // Take current health and change that value into 0-1
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        /*
         * Example: Current health out of 100 is 50... outMin is 0, outMax is 1
         * (50 - 0) * (1 - 0) / (100 - 0) + 0
         * = 50 * 1 / 100 = 0.8 value
         */

        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
