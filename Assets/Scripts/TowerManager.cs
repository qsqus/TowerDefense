using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    public static TowerManager instance { get; private set; }

    private GameObject towerToBuild;
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
        towerToBuild = towerPrefab;
    }

    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }


}
