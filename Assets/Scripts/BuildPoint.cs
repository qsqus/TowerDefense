using System.Collections;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    [Header("Build point stats")]
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private float radius = 0.5f;

    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";

    [Header("References")]
    [SerializeField] private Renderer buildPointRenderer;
    [SerializeField] private MeshFilter buildPointMeshFilter;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private DropCollectibles dropCollectibles;

    private Material[] selectedMaterials;
    private Material[] selectedMaterials3;

    private GameObject towerObject;
    private Tower tower;
    private PlayerBuild playerBuild;
    private Material[] startMaterials;

    private Renderer[] towerRenderers;
    private Material[][] towerStartMaterials;

    private int instanceID;


    void Start()
    {
        modelTransform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        modelTransform.localScale *= Random.Range(0.9f, 1.1f);

        buildPointMeshFilter.mesh = Instantiate(BuildPointVisualManager.instance.GetRandomMesh());

        Material[] randomMaterials = BuildPointVisualManager.instance.GetRandomMaterials();

        if (BuildPointVisualManager.instance.IsReversedMesh(buildPointMeshFilter.mesh.name))
        {
            Material temp = randomMaterials[0];
            randomMaterials[0] = randomMaterials[1];
            randomMaterials[1] = temp;
        }

        buildPointRenderer.materials = randomMaterials;


        playerBuild = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerBuild>();
        playerBuild.OnInteractPressed += PlayerBuild_OnInteractPressed;

        TowerManager.instance.OnTowerToBuildSelected += TowerManager_OnTowerToBuildSelected;

        instanceID = gameObject.GetInstanceID();

        startMaterials = buildPointRenderer.materials;
        selectedMaterials = new Material[] { selectedMaterial, selectedMaterial };
        selectedMaterials3 = new Material[] { selectedMaterial, selectedMaterial, selectedMaterial };
    
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
                ExitBuildPoint();

                dropCollectibles.DropAmountOfCollectibles(LevelManager.instance.coin, tower.GetResellPrice() / LevelManager.instance.GetCoinWorth());

                Debug.Log("Edit");


                Destroy(towerObject);
                towerObject = null;
                tower = null;

                // Makes tree stump visible
                //renderers[1].enabled = true;
                buildPointRenderer.enabled = true;
            }
        }
    }

    // Builds tower on build point
    private void BuildTower(GameObject towerToBuild)
    {
        towerObject = Instantiate(towerToBuild, transform.position, transform.rotation);
        tower = towerObject.GetComponent<Tower>();
        
        towerRenderers = tower.GetRenderers();
        towerStartMaterials = new Material[towerRenderers.Length][];

        // Makes tree stump not visible
        //renderers[1].enabled = false;

        for (int i = 0; i < towerRenderers.Length; i++)
        {
            towerStartMaterials[i] = towerRenderers[i].materials;
        }

        EnterBuildPoint();

        // Makes tree crown renderer not visible
        //renderers[0].enabled = false;
        buildPointRenderer.enabled = false;

    }

    // Player entered/selected build point
    public void EnterBuildPoint()
    {
        if (HasTower())
        {
            for (int i = 0; i < towerRenderers.Length; i++)
            {
                int materialsAmount = towerRenderers[i].materials.Length;
                if(materialsAmount == 1)
                {
                    towerRenderers[i].material = selectedMaterial;
                }
                else if (materialsAmount == 2)
                {
                    towerRenderers[i].materials = selectedMaterials;
                }
                else
                {
                    towerRenderers[i].materials = selectedMaterials3;
                }

            }
            tower.StartUpgrading();
            tower.ToggleProgressBar(true);

            TowerRangeDisplayManager.instance.ShowTowerRange(towerObject.transform.position, tower.GetTowerRange());

            return;
        }

        buildPointRenderer.materials = selectedMaterials;

    }

    // Player exited/deselected build point
    public void ExitBuildPoint()
    {
        TowerManager.instance.AttemptHideTowerBuildMenu();

        if (HasTower())
        {
            for (int i = 0; i < towerRenderers.Length; i++)
            {
                towerRenderers[i].materials = towerStartMaterials[i];
            }
            tower.StopUpgrading();
            tower.ToggleProgressBar(false);

            TowerRangeDisplayManager.instance.HideTowerRange();

            return;
        }

        buildPointRenderer.materials = startMaterials;

    }

    // Returns radius of build point
    public float GetRadius()
    {
        return radius;
    }

    public bool HasTower()
    {
        return towerObject != null;
    }

}
