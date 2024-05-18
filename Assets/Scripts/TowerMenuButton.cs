using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private Image highlightedFrame;
    [SerializeField] private Image frame;
    [SerializeField] private TMP_Text towerPrice;
    [SerializeField] private float scaleMultiplier = 1.1f;


    private RectTransform rectTransform;
    private Tower towerScript;
    private Vector3 startScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        towerScript = tower.GetComponent<Tower>();

    }

    private void Start()
    {
        highlightedFrame.enabled = false;
        towerPrice.text = towerScript.GetPrice().ToString();
        startScale = transform.localScale;
    }

    // Returns selected tower
    public GameObject GetTower()
    {
        NormalVisual();
        return tower;
    }

    public void HighlightedVisual()
    {
        highlightedFrame.enabled = true;
        transform.localScale *= scaleMultiplier;
        TowerRangeDisplayManager.instance.ShowTowerRange(TowerManager.instance.GetSelectedBuildPointPosition(), towerScript.GetTowerRange());
        TowerManager.instance.UpdateTowerInfo(towerScript.GetPrice(), towerScript.GetTargetedEnemyTypes());
    }

    public void NormalVisual()
    {
        highlightedFrame.enabled = false;
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
