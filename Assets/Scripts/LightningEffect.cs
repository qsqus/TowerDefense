using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    [SerializeField] private Transform lightningEffect;
    
    public void MultiplyScale(float scaleMultiplier)
    {
        lightningEffect.transform.localScale *= scaleMultiplier;
    }

}
