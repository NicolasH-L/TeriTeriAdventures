using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        print(_slider == null);
    }

    public void SetMaxValue(int maxValue)
    {
        print(_slider.maxValue.ToString());
        _slider.maxValue = maxValue;
    }

    public void SetValue(int value)
    {
        _slider.value = value;
    }
}