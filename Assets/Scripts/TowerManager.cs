using System;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private float displayHeight = 6f;
    [SerializeField] private string cameraTag = "MainCamera";
    [SerializeField] private Transform buttonContainer;

    public static TowerManager instance { get; private set; }
    
    public event Action<GameObject, int> OnTowerToBuildSelected;

    private bool isBuildMenuOpen = false;
    private GameObject selectedBuildPoint;
    private GameObject cam;
    private TowerMenuButton[] buttons;
    private int currentButtonIdx;

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
        buttons = new TowerMenuButton[buttonContainer.childCount];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = buttonContainer.GetChild(i).GetComponent<TowerMenuButton>();
        }

        cam = GameObject.FindGameObjectWithTag(cameraTag);
        
        HideTowerBuildMenu();
    }

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if(scrollWheel > 0)
        {
            buttons[currentButtonIdx].NormalVisual();
            currentButtonIdx += 1;
            if(currentButtonIdx > buttons.Length - 1)
            {
                currentButtonIdx = 0;
            }
            buttons[currentButtonIdx].HighlightedVisual();

        }
        else if(scrollWheel < 0)
        {
            buttons[currentButtonIdx].NormalVisual();
            currentButtonIdx -= 1;
            if (currentButtonIdx < 0)
            {
                currentButtonIdx = buttons.Length - 1;
            }
            buttons[currentButtonIdx].HighlightedVisual();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectTowerToBuild(buttons[currentButtonIdx].GetTower());
        }
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

    public void AttemptHideTowerBuildMenu()
    {
        if(isBuildMenuOpen)
        {
            HideTowerBuildMenu();
        }
    }

    // Hides tower build menu
    public void HideTowerBuildMenu()
    {
        if(selectedBuildPoint != null && !selectedBuildPoint.GetComponent<BuildPoint>().HasTower())
        {
            TowerRangeDisplayManager.instance.HideTowerRange();
        }

        gameObject.SetActive(false);
        selectedBuildPoint = null;
        isBuildMenuOpen = false;

        buttons[currentButtonIdx].NormalVisual();
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

        currentButtonIdx = (int)(buttons.Length / 2f);
        buttons[currentButtonIdx].HighlightedVisual();
    }

    public Vector3 GetSelectedBuildPointPosition()
    {
        return selectedBuildPoint.transform.position;
    }
}
