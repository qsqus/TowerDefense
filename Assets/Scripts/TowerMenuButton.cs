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

    private Tower towerScript;
    private Vector3 startScale;

    private void Start()
    {
        highlightedFrame.enabled = false;
        towerScript = tower.GetComponent<Tower>();
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
    }

    public void NormalVisual()
    {
        highlightedFrame.enabled = false;
        transform.localScale = startScale;

    }

}
