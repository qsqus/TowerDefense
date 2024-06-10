using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private Image frame;
    [SerializeField] private TMP_Text towerPrice;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color startColor;
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Vector3 startScale = new Vector3(1.6f, 1.6f, 1.6f);

    private RectTransform rectTransform;
    private Tower towerScript;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        towerScript = tower.GetComponent<Tower>();
    }

    // private void Start()
    private void OnEnable()
    {
        towerPrice.text = towerScript.GetPrice().ToString();
        frame.color = startColor;
        //NormalVisual();
    }

    // Returns selected tower
    public GameObject GetTower()
    {
        return tower;
    }

    public void HighlightedVisual()
    {
        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.towerMenuSwitched, transform);

        frame.color = selectedColor;
        transform.localScale *= scaleMultiplier;
        TowerRangeDisplayManager.instance.ShowTowerRange(TowerManager.instance.GetSelectedBuildPointPosition(), towerScript.GetTowerRange(), false);
        TowerManager.instance.UpdateTowerInfo(towerScript.GetPrice(), towerScript.GetTargetedEnemyTypes());

    }

    public void NormalVisual()
    {
        frame.color = startColor;
        transform.localScale = startScale;
    }

    public void RotateByAngle(float angle)
    {
        Vector3 currentRotation = rectTransform.localEulerAngles;
        currentRotation.z += angle;
        rectTransform.localEulerAngles = currentRotation;
    }

    public void RestartRotation()
    {
        rectTransform.localEulerAngles = Vector3.zero;
    }

}
