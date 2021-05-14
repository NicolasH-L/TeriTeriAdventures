using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider _slider;
    private int _currentMaxValue;
    private int _currentValue;
    
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetMaxValue(int maxValue)
    {
        _slider.maxValue = maxValue;
        _currentMaxValue = maxValue;
    }

    public void SetValue(int value)
    {
        _slider.value = value;
        _currentValue = value;
    }

    public int GetCurrentMaxValue()
    {
        return _currentMaxValue;
    }

    public int GetCurrentValue()
    {
        return _currentValue;
    }
}