using System;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Tower Select")]
    [SerializeField] private float displayHeight = 6f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float wheelRotationSpeed = 10f;
    [SerializeField] private int rotationDirection = 1;
    [SerializeField] private float zMaxPosition = 5f;
    [SerializeField] private float zMinPosition = -5f;
    [SerializeField] private float xMaxPosition = 10f;
    [SerializeField] private float xMinPosition = -10f;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private TMP_Text priceDisplay;
    [SerializeField] private TMP_Text targetedEnemyTypeDisplay;
    [SerializeField] private string cameraTag = "MainCamera";

    public static TowerManager instance { get; private set; }
    
    public event Action<GameObject, int> OnTowerToBuildSelected;

    private bool isBuildMenuOpen = false;
    private GameObject selectedBuildPoint;
    private GameObject cam;
    private TowerMenuButton[] buttons;
    private int currentButtonIdx = 0;

    private float angle;
    private RectTransform buttonContainerRect;
    private float targetRotation;


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of TowerManager in scene");
            
            Destroy(gameObject);
            return;
        }
        instance = this;

        buttonContainerRect = buttonContainer.GetComponent<RectTransform>();

    }

    private void Start()
    {
        RadialArrange();

        cam = GameObject.FindGameObjectWithTag(cameraTag);
        
        HideTowerBuildMenu();
    }

    private void Update()
    {
        // Smoothly rotates the wheel
        if(targetRotation != 0f)
        {
            float currentAngle = targetRotation * Time.deltaTime * wheelRotationSpeed;
            RotateWheel(currentAngle);
            targetRotation -= currentAngle;
        }

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * rotationDirection;
        if(scrollWheel > 0)
        {
            buttons[currentButtonIdx].NormalVisual();
            currentButtonIdx -= 1;
            if (currentButtonIdx < 0)
            {
                currentButtonIdx = buttons.Length - 1;
            }
            buttons[currentButtonIdx].HighlightedVisual();
            targetRotation += angle;
        }
        else if(scrollWheel < 0)
        {
            buttons[currentButtonIdx].NormalVisual();
            currentButtonIdx += 1;
            if (currentButtonIdx > buttons.Length - 1)
            {
                currentButtonIdx = 0;
            }
            buttons[currentButtonIdx].HighlightedVisual();
            targetRotation -= angle;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectTowerToBuild(buttons[currentButtonIdx].GetTower());
        }
    }

    private void RotateWheel(float rotationAngle)
    {
        rotationAngle *= Mathf.Rad2Deg;

        Vector3 currentRotation = buttonContainerRect.localEulerAngles;
        currentRotation.z += rotationAngle;
        buttonContainerRect.localEulerAngles = currentRotation;

        foreach (TowerMenuButton button in buttons)
        {
            button.RotateByAngle(-rotationAngle);
        }

    }

    private void RestartRotation()
    {
        targetRotation = 0f;
        buttonContainerRect.localEulerAngles = Vector3.zero;

        foreach (TowerMenuButton button in buttons)
        {
            button.RestartRotation();
        }
    }

    // Arranges children of buttonContainer in circle
    private void RadialArrange()
    {
        int amountOfButtons = buttonContainer.childCount;
        buttons = new TowerMenuButton[amountOfButtons];
        angle = 2f * Mathf.PI / amountOfButtons;

        for (int i = 0; i < amountOfButtons; i++)
        {
            buttons[i] = buttonContainer.GetChild(i).GetComponent<TowerMenuButton>();

            float newAngle = (i + 1) * angle;

            RectTransform buttonRect = buttons[i].GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(radius * Mathf.Cos(newAngle), radius * (float)Math.Sin(newAngle));

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
                buttons[currentButtonIdx].NormalVisual();

                SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.towerToBuildSelected, selectedBuildPoint.transform);

                LevelManager.instance.ChangeCoinsByAmount(-towerPrice, true);
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
        RestartRotation();
    }

    // Shows tower build menu, buildPoint - selected build point
    public void ShowTowerBuildMenu(GameObject buildPoint)
    {
        if(isBuildMenuOpen)
        {
            HideTowerBuildMenu();
            return;
        }

        // Move the menu so that it fits on screen
        Vector3 displayPosition = buildPoint.transform.position + new Vector3(0, displayHeight, 0);

        if (displayPosition.x < xMinPosition)
        {
            displayPosition.x = xMinPosition;
        }
        else if(displayPosition.x > xMaxPosition)
        {
            displayPosition.x = xMaxPosition;
        }

        if (displayPosition.z < zMinPosition)
        {
            displayPosition.z = zMinPosition;
        }
        else if (displayPosition.z > zMaxPosition)
        {
            displayPosition.z = zMaxPosition;
        }

        //transform.position = buildPoint.transform.position + new Vector3(0, displayHeight, 0); 
        transform.position = displayPosition;

        Debug.Log(transform.position);

        // Looks at camera
        transform.LookAt(transform.position + cam.transform.forward);

        gameObject.SetActive(true);
        selectedBuildPoint = buildPoint;
        isBuildMenuOpen = true;

        SoundEffectsManager.instance.PlaySoundEffectClip(SoundEffectsManager.instance.towerMenuOpened, selectedBuildPoint.transform);

        currentButtonIdx = 0;
        buttons[currentButtonIdx].HighlightedVisual();
    }

    public Vector3 GetSelectedBuildPointPosition()
    {
        return selectedBuildPoint.transform.position;
    }

    public void UpdateTowerInfo(int price, EnemyType[] enemyType)
    {
        priceDisplay.text = price.ToString();

        if (enemyType.Length > 1)
        {
            targetedEnemyTypeDisplay.text = "All";
        }
        else
        {
            targetedEnemyTypeDisplay.text = enemyType[0].ToString();
        }
    }

}
