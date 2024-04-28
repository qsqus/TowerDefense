using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    private Slider slider;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Sets sldiers max value
    public void SetMaxValue(float amount)
    {
        slider.maxValue = amount;
        slider.value = amount;

        fill.color = gradient.Evaluate(1f);
    }

    // Sets sliders value
    public void SetValue(float amount)
    {
        slider.value = amount;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
  
}
