using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] bool isHealthBar = true;

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

        if (isHealthBar && amount <= 0)
        {
            gameObject.SetActive(false);
        }
        //StartCoroutine(FillSlider(amount, 0.2f));

    }

    // Fills slider to a given amount over given time
    private IEnumerator FillSlider(float targetValue, float duration)
    {
        float startValue = slider.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            fill.color = gradient.Evaluate(slider.normalizedValue);
            yield return null;
        }

        slider.value = targetValue;

        if (targetValue <= 0)
        {
            gameObject.SetActive(false);
        }
    }
  
}
