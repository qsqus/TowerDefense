using System;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] float displayHeight = 6f;
    [SerializeField] private string cameraTag = "MainCamera";

    public static TowerManager instance { get; private set; }
    
    public event Action<GameObject, int> OnTowerToBuildSelected;

    private bool isBuildMenuOpen = false;
    private GameObject selectedBuildPoint;
    private GameObject cam;

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
        cam = GameObject.FindGameObjectWithTag(cameraTag);
        HideTowerBuildMenu();
    }

    // Selects tower to build
    public void SelectTowerToBuild(GameObject tower)
    {
        int towerPrice = tower.GetComponent<Tower>().GetPrice();
        if (towerPrice <= LevelManager.instance.GetCoinsAmount())
        {
            if(!selectedBuildPoint.GetComponent<BuildPoint>().HasTower())
            {
                LevelManager.instance.ChangeCoinsByAmount(-towerPrice);
                OnTowerToBuildSelected?.Invoke(tower, selectedBuildPoint.GetInstanceID());
                HideTowerBuildMenu();
            }
        }
        else
        {
            Debug.Log("Not enough coins");
        }

    }

    // Hides tower build menu
    public void HideTowerBuildMenu()
    {
        gameObject.SetActive(false);
        selectedBuildPoint = null;
        isBuildMenuOpen = false;
    }

    // Shows tower build menu, buildPoint - selected build point
    public void ShowTowerBuildMenu(GameObject buildPoint)
    {
        if(isBuildMenuOpen)
        {
            HideTowerBuildMenu();
            return;
        }

        transform.position = buildPoint.transform.position + new Vector3(0, displayHeight, 0); 
        
        // Looks at camera
        transform.LookAt(transform.position + cam.transform.forward);

        gameObject.SetActive(true);
        selectedBuildPoint = buildPoint;
        isBuildMenuOpen = true;
    }

}
