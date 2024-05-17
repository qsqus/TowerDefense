using System.Collections;
using UnityEngine;

public class MaterialFlash : MonoBehaviour
{
    [SerializeField] private int flashAmount = 9;
    [SerializeField] private Color flashColor;
    [SerializeField] private Renderer[] renderers;

    private bool hasFinishedFlashing = false;

    public void StartFlashing(float duration)
    {
        StartCoroutine(Flash(duration, flashColor, flashAmount));
    }

    private IEnumerator Flash(float duration, Color flashColor, int flashAmount)
    {
        Color[] startColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            startColors[i] = renderers[i].material.color;
        }

        float elapsedFlashTime = 0f;
        float elapsedFlashPercentage = 0f;

        while(elapsedFlashTime < duration)
        {
            elapsedFlashTime += Time.deltaTime;
            elapsedFlashPercentage = elapsedFlashTime / duration;

            elapsedFlashPercentage = (elapsedFlashPercentage > 1) ? 1 : elapsedFlashPercentage;
            
            float pingPongPercentage = Mathf.PingPong(elapsedFlashPercentage * 2 * flashAmount, 1);

            for(int i= 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.Lerp(startColors[i], flashColor, pingPongPercentage);
            }

            yield return null;
        }

        hasFinishedFlashing = true;
    }

    public bool HasFinishedFlashing()
    {
        return hasFinishedFlashing;
    }

}
