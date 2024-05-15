using System.Collections;
using UnityEngine;

public class MaterialFlash : MonoBehaviour
{
    [SerializeField] private int flashAmount = 9;
    [SerializeField] private Color flashColor;
    [SerializeField] private Renderer rend;

    private bool hasFinishedFlashing = false;

    public void StartFlashing(float duration)
    {
        StartCoroutine(Flash(duration, flashColor, flashAmount));
    }

    private IEnumerator Flash(float duration, Color flashColor, int flashAmount)
    {
        Color startColor = rend.material.color;
        float elapsedFlashTime = 0f;
        float elapsedFlashPercentage = 0f;

        while(elapsedFlashTime < duration)
        {
            elapsedFlashTime += Time.deltaTime;
            elapsedFlashPercentage = elapsedFlashTime / duration;

            elapsedFlashPercentage = (elapsedFlashPercentage > 1) ? 1 : elapsedFlashPercentage;
            
            float pingPongPercentage = Mathf.PingPong(elapsedFlashPercentage * 2 * flashAmount, 1);
            rend.material.color = Color.Lerp(startColor, flashColor, pingPongPercentage);

            yield return null;
        }

        hasFinishedFlashing = true;
    }

    public bool HasFinishedFlashing()
    {
        return hasFinishedFlashing;
    }

}
