using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    private Turret turret;
    private GameObject buildPoint;
    private bool canBuild = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(canBuild)
            {
                Vector3 position = buildPoint.transform.position;
                Quaternion rotation = buildPoint.transform.rotation;
                
                Destroy(buildPoint);
                buildPoint = null;
                canBuild = false;

                Instantiate(turret, position, rotation);
            }
        }
    }

    public void SetBuildPoint(GameObject buildPoint)
    {
        this.buildPoint = buildPoint;
    }

    public void SetTurret(Turret turret)
    {
        this.turret = turret;
    }

    public void SetBuildPermission(bool canBuild)
    {
        this.canBuild = canBuild;
    }

}
