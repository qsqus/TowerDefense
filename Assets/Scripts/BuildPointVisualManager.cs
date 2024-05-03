using System.Collections.Generic;
using UnityEngine;

public class BuildPointVisualManager : MonoBehaviour
{
    [SerializeField] private Mesh[] buildPointMeshes;
    [SerializeField] private Material[] leavesMaterials;
    [SerializeField] private Material[] barkMaterials;

    public static BuildPointVisualManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of BuildPointVisualManager in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public Mesh GetRandomMesh()
    {
        return buildPointMeshes[Random.Range(0, buildPointMeshes.Length)];

    }

    public Material[] GetRandomMaterials()
    {
        Material[] materials = new Material[2];
        materials[0] = leavesMaterials[Random.Range(0, leavesMaterials.Length)];
        materials[1] = barkMaterials[Random.Range(0, barkMaterials.Length)];

        return materials;
    }

}
