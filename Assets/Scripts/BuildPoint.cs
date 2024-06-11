using System.Collections;
using UnityEngine;

public class BuildPoint : MonoBehaviour
{
    [Header("Build point stats")]
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float popDuration = 0.1f;
    [SerializeField] private float popMultiplier = 1.1f;

    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";

    [Header("References")]
    [SerializeField] private Renderer buildPointRenderer;
    [SerializeField] private MeshFilter buildPointMeshFilter;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private DropCollectibles dropCollectibles;

    [Header("To remove on awake")]
    [SerializeField] private GameObject towerAligner;

    private Material[] selectedMaterials;
    private Material[] selectedMaterials3;

    private GameObject towerObject;
    private Tower tower;
    private PlayerBuild playerBuild;
    private Material[] startMaterials;

    private Renderer[] towerRenderers;
    private Material[][] towerStartMaterials;

    private int instanceID;

    private void Awake()
    {
        Destroy(towerAligner.gameObject);
    }
    private void Start()
    {
        modelTransform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        modelTransform.localScale *= Random.Range(0.9f, 1.1f);

        buildPointMeshFilter.mesh = Instantiate(EnvironmentVisualManager.instance.GetRandomBuildPointMesh());

        Material[] randomMaterials = EnvironmentVisualManager.instance.GetRandomBuildPointMaterials();

        if (EnvironmentVisualManager.instance.IsReversedMesh(buildPointMeshFilter.mesh.name))
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

                tower.ShowBuildEffect();
                tower.DestroyTower();
                towerObject = null;
                tower = null;

                EnterBuildPoint();

                // Makes tree stump visible
                //renderers[1].enabled = true;
                buildPointRenderer.enabled = true;
            }
        }
    }

    // Builds tower on build point
    private void BuildTower(GameObject towerToBuild)
    {
        ExitBuildPoint();
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
            tower.ToggleLevelDisplay(true);
            tower.ToggleIsSelected(true);

            SoundEffectsManager.instance.PlayRandomSoundEffectClip(SoundEffectsManager.instance.towerEntered, transform);


            TowerRangeDisplayManager.instance.ShowTowerRange(towerObject.transform.position, tower.GetTowerRange());

            StartCoroutine(MakeObjectPop(towerObject, popMultiplier, popDuration));

            return;
        }

        SoundEffectsManager.instance.PlayRandomSoundEffectClip(SoundEffectsManager.instance.buildPointEntered, transform);
        buildPointRenderer.materials = selectedMaterials;
        StartCoroutine(MakeObjectPop(gameObject, popMultiplier, popDuration));

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
            tower.ToggleLevelDisplay(false);
            tower.ToggleIsSelected(false);

            TowerRangeDisplayManager.instance.HideTowerRange();

            return;
        }

        buildPointRenderer.materials = startMaterials;

    }

    // Makes object pop
    private IEnumerator MakeObjectPop(GameObject obj, float multiplier, float time)
    {
        Vector3 startScale = obj.transform.localScale;
        Vector3 targetScale = startScale * multiplier;

        // Scale up
        yield return StartCoroutine(ScaleOverTime(obj, startScale, targetScale, time));

        // Scale down
        yield return StartCoroutine(ScaleOverTime(obj, targetScale, startScale, time));
    }

    // Scales object over time
    private IEnumerator ScaleOverTime(GameObject obj, Vector3 startScale, Vector3 endScale, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            // Prevents an error when object is destroyed during couroutine
            if(Object.ReferenceEquals(obj, null) || obj == null)
            {
                yield break;
            }

            obj.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (!(Object.ReferenceEquals(obj, null) || obj == null))
        {
            obj.transform.localScale = endScale;
        }
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
