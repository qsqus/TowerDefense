using System;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    [SerializeField] private float playerRadius;
    [SerializeField] private float rangeOffset = 0.1f;
    [SerializeField] private string buildPointTag = "BuildPoint";

    private GameObject selectedBuildPoint;
    private bool canEdit = false;
    private GameObject[] buildPoints;
    private float range;
    private bool canBuild = true;

    public event Action<int> OnInteractPressed;

    void Start()
    {
        buildPoints = GameObject.FindGameObjectsWithTag(buildPointTag);
        range = buildPoints[0].GetComponent<BuildPoint>().GetRadius() + playerRadius - rangeOffset;

        InvokeRepeating("UpdateSelectedBuildPoint", 0f, 0.1f);
    }

    void Update()
    {
        GatherActionInput();
    }

    // Selects build point that is nearest to player and in range - called every 0.1s
    private void UpdateSelectedBuildPoint()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestBuildPoint = null;

        foreach (GameObject buildPoint in buildPoints)
        {
            //float distance = Vector3.Distance(transform.position, buildPoint.transform.position);
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(buildPoint.transform.position.x, buildPoint.transform.position.z));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestBuildPoint = buildPoint;
            }
        }

        if (nearestBuildPoint != null && shortestDistance <= range)
        {
            if (selectedBuildPoint != nearestBuildPoint)
            {
                if (selectedBuildPoint != null)
                {
                    selectedBuildPoint.GetComponent<BuildPoint>().ExitBuildPoint();
                }
                canEdit = true;
                selectedBuildPoint = nearestBuildPoint;
                nearestBuildPoint.GetComponent<BuildPoint>().EnterBuildPoint();
            }
        }
        else if (selectedBuildPoint != null)
        {
            selectedBuildPoint.GetComponent<BuildPoint>().ExitBuildPoint();
            canEdit = false;
            selectedBuildPoint = null;
        }
    }

    // Gathers non movement input
    private void GatherActionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canEdit && canBuild)
            {
                OnInteractPressed?.Invoke(selectedBuildPoint.GetInstanceID());
            }
        }
    }

    public void SetCanBuild(bool canBuild)
    {
        this.canBuild = canBuild;
    }

}
