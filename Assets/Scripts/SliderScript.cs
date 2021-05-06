using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxValue(int maxValue)
    {
        slider.maxValue = maxValue;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }
}