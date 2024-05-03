using System;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance { get; private set; }

    public event Action<GameObject, int> OnTowerToBuildSelected;

    private GameObject selectedBuildPoint;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of TowerManager in scene");
            
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        HideTowerBuildMenu();
    }

    // Selects tower to build
    public void SelectTowerToBuild(GameObject tower)
    {
        if(!selectedBuildPoint.GetComponent<BuildPoint>().HasTower())
        {
            OnTowerToBuildSelected?.Invoke(tower, selectedBuildPoint.GetInstanceID());
        }

    }

    // Hides tower build menu
    public void HideTowerBuildMenu()
    {
        gameObject.SetActive(false);
        selectedBuildPoint = null;

    }

    // Shows tower build menu, buildPoint - selected build point
    public void ShowTowerBuildMenu(GameObject buildPoint)
    {
        gameObject.SetActive(true);
        selectedBuildPoint = buildPoint;

    }

}
