using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightChange : MonoBehaviour
{
    [SerializeField] private Light lightObject;
    [SerializeField] private Slider lightSlider;
    [SerializeField] private float inputRange;
    [SerializeField] private float outputRange; 


    // Update is called once per frame
    public void UpdateLight()
    {
        lightObject.intensity = Map(lightSlider.value, inputRange, outputRange);       

    }

    float Map(float inputValue,float inputValueRange, float outputValueRange)
    {
        float outputValue = inputValue * (outputValueRange / inputValueRange);

        return outputValue;
    }
}
