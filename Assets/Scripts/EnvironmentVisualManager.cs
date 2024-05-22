using UnityEngine;

public class EnvironmentVisualManager : MonoBehaviour
{
    [Header("Build point")]
    [SerializeField] private Mesh[] buildPointMeshes;
    [SerializeField] private Material[] leavesMaterials;
    [SerializeField] private Material[] barkMaterials;
    [SerializeField] private string[] reversedMeshNames;

    [Header("Grass and flowers")]
    [SerializeField] private Mesh[] flowerMeshes;
    [SerializeField] private Mesh[] grassMeshes;
    [SerializeField] private Material[] greenMaterials;
    [SerializeField] private Material[] colorfulMaterials;


    public static EnvironmentVisualManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of EnvironmentVisualManager in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Mesh GetRandomBuildPointMesh()
    {
        return buildPointMeshes[Random.Range(0, buildPointMeshes.Length)];

    }

    public Material[] GetRandomBuildPointMaterials()
    {
        Material[] materials = new Material[2];
        materials[0] = leavesMaterials[Random.Range(0, leavesMaterials.Length)];
        materials[1] = barkMaterials[Random.Range(0, barkMaterials.Length)];

        return materials;
    }

    // Checks if build point mesh has reversed order of materials
    public bool IsReversedMesh(string name)
    {
        foreach (string rname in reversedMeshNames)
        {
            if(rname + "(Clone) Instance"== name)
            {
                return true;
            }
        }
        return false;
    }

    public Mesh GetRandomFlowerMesh()
    {
        return flowerMeshes[Random.Range(0, flowerMeshes.Length)];

    }

    public Material[] GetRandomFlowerMaterials()
    {
        Material[] materials = new Material[2];
        materials[0] = GetRandomGrassMaterial();
        materials[1] = colorfulMaterials[Random.Range(0, colorfulMaterials.Length)];

        return materials;
    }

    public Mesh GetRandomGrassFlowerMesh()
    {
        return grassMeshes[Random.Range(0, grassMeshes.Length)];

    }

    public Material GetRandomGrassMaterial()
    {
        return greenMaterials[Random.Range(0, greenMaterials.Length)];
    }


}
