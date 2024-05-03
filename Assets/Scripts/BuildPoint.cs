using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    [SerializeField] private float selectedColorAlpha = 0.5f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Renderer[] renderers;

    private Color[] selectedColors;
    private Color[] startColors;

    private GameObject tower;
    private PlayerBuild playerBuild;

    private Renderer[] towerRenderers;
    private Color[] towerSelectedColors;
    private Color[] towerStartColors;

    private int instanceID;


    void Start()
    {
        playerBuild = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerBuild>();
        playerBuild.OnInteractPressed += PlayerBuild_OnInteractPressed;

        TowerManager.instance.OnTowerToBuildSelected += TowerManager_OnTowerToBuildSelected;

        instanceID = gameObject.GetInstanceID();

        selectedColors = new Color[renderers.Length];
        startColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            startColors[i] = renderers[i].material.color;
            selectedColors[i] = startColors[i];
            selectedColors[i].a = selectedColorAlpha;
        }
    
    }

    private void TowerManager_OnTowerToBuildSelected(GameObject towerToBuild, int buildPointInstanceID)
    {
        if(instanceID == buildPointInstanceID)
        {
            BuildTower(towerToBuild);
        }
    }

    private void PlayerBuild_OnInteractPressed(int buildPointInstanceID)
    {
        if (instanceID == buildPointInstanceID)
        {
            if (!HasTower())
            {
                Debug.Log("Show tower selector menu");
                TowerManager.instance.ShowTowerBuildMenu(gameObject);
                //BuildTower();
            }
            else
            {
                Debug.Log("Edit");
                Destroy(tower);

                // Makes tree stump visible
                renderers[1].enabled = true;
            }
        }
    }

    // Builds tower on build point
    private void BuildTower(GameObject towerToBuild)
    {
        tower = Instantiate(towerToBuild, transform.position - new Vector3(0, 1f, 0), transform.rotation);   // weird offset here - not okay
        
        towerRenderers = tower.GetComponent<Tower>().GetRenderers();
        towerStartColors = new Color[towerRenderers.Length];
        towerSelectedColors = new Color[towerRenderers.Length];

        // Makes tree stump not visible
        renderers[1].enabled = false;

        for (int i = 0; i < towerRenderers.Length; i++)
        {
            towerStartColors[i] = towerRenderers[i].material.color;
            towerSelectedColors[i] = towerStartColors[i];
            towerSelectedColors[i].a = selectedColorAlpha;
        }

        EnterBuildPoint();

        // Makes tree crown renderer not visible
        renderers[0].enabled = false;

    }

    // Player entered/selected build point
    public void EnterBuildPoint()
    {
        if(HasTower())
        {
            for (int i = 0; i < towerRenderers.Length; i++)
            {
                towerRenderers[i].material.color = towerSelectedColors[i];
            }

            return;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = selectedColors[i];
        }

    }

    // Player exited/deselected build point
    public void ExitBuildPoint()
    {
        TowerManager.instance.HideTowerBuildMenu();

        if (HasTower())
        {
            for (int i = 0; i < towerRenderers.Length; i++)
            {
                towerRenderers[i].material.color = towerStartColors[i];
            }

            return;
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = startColors[i];
        }

    }

    // Returns radius of build point
    public float GetRadius()
    {
        return radius;
    }

    public bool HasTower()
    {
        return tower != null;
    }

}
