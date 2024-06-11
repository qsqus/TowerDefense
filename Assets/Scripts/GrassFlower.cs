using UnityEngine;

public class GrassFlower : MonoBehaviour
{
    [SerializeField] private bool isFlower;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Renderer rend;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float minScale = 1.2f;
    [SerializeField] private float maxScale = 1.7f;

    void Start()
    {
        modelTransform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        modelTransform.localScale *= Random.Range(minScale, maxScale);

        if(isFlower)
        {
            meshFilter.mesh = EnvironmentVisualManager.instance.GetRandomFlowerMesh();
            rend.materials = EnvironmentVisualManager.instance.GetRandomFlowerMaterials();
        }
        else
        {
            meshFilter.mesh = EnvironmentVisualManager.instance.GetRandomGrassFlowerMesh();
            rend.material = EnvironmentVisualManager.instance.GetRandomGrassMaterial();
        }
    }
    
}
