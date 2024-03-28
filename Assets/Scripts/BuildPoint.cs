using UnityEngine;
using UnityEngine.XR.WSA;
using static UnityEngine.GraphicsBuffer;

public class BuildPoint : MonoBehaviour
{
    [SerializeField] private float selectedColorAlpha = 0.5f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Turret buildTurret;

    private Renderer rend;
    private Color selectedColor;
    private Color startingColor;
    void Start()
    {
        rend = GetComponent<Renderer>();
        startingColor = rend.material.color;
        selectedColor = startingColor;
        selectedColor.a = selectedColorAlpha;
    }

    private void OnTriggerEnter(Collider other)
    {
        TowerBuildManager playerTowerBuildManager = other.transform.GetComponent<TowerBuildManager>();
        rend.material.color = selectedColor;
        HandlePlayerTirggerEnter(other, true, gameObject, buildTurret);

    }

    private void OnTriggerExit(Collider other)
    {
        rend.material.color = startingColor;
        HandlePlayerTirggerEnter(other, false, null, null);

    }

    private void HandlePlayerTirggerEnter(Collider collider, bool canBuild, GameObject buildPoint, Turret turret)
    {
        if (collider.CompareTag(playerTag))
        {
            TowerBuildManager playerTowerBuildManager = collider.transform.GetComponent<TowerBuildManager>();
            
            playerTowerBuildManager.SetBuildPermission(canBuild);
            playerTowerBuildManager.SetBuildPoint(buildPoint);
            playerTowerBuildManager.SetTurret(turret);

        }
    }
}
