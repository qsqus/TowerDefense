using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private Image highlightedFrame;
    [SerializeField] private Image frame;
    [SerializeField] private TMP_Text towerPrice;

    private Tower towerScript;

    private void Start()
    {
        highlightedFrame.enabled = false;
        towerScript = tower.GetComponent<Tower>();
        towerPrice.text = towerScript.GetPrice().ToString();
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
        TowerRangeDisplayManager.instance.ShowTowerRange(TowerManager.instance.GetSelectedBuildPointPosition(), towerScript.GetTowerRange());
    }

    public void NormalVisual()
    {
        highlightedFrame.enabled = false;
    }

}
