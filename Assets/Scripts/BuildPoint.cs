using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.WSA;
using static UnityEngine.GraphicsBuffer;

public class BuildPoint : MonoBehaviour
{
    [SerializeField] private float selectedColorAlpha = 0.5f;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float radius = 0.5f;

    private Renderer rend;
    private Color selectedColor;
    private Color startColor;

    private GameObject tower;
    private PlayerBuild playerBuild;

    void Start()
    {
        playerBuild = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerBuild>();
        playerBuild.OnInteractPressed += PlayerBuild_OnInteractPressed;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        selectedColor = startColor;
        selectedColor.a = selectedColorAlpha;
    }

    private void PlayerBuild_OnInteractPressed(GameObject obj)
    {
        if (gameObject == obj)
        {
            if (tower == null)
            {
                BuildTower();
            }
            else
            {
                Debug.Log("Edit");
                Destroy(tower);
            }
        }
    }

    // Builds tower on build point
    private void BuildTower()
    {
        GameObject selectedTower = TowerManager.instance.GetTowerToBuild();
        tower = Instantiate(selectedTower, transform.position - new Vector3(0, 1f, 0), transform.rotation);   // weird offset here - not okay
        // Makes rendered not visible
        rend.enabled = false;

    }

    // Player entered/selected build point
    public void EnterBuildPoint()
    {
        rend.material.color = selectedColor;
    }

    // Player exited/deselected build point
    public void ExitBuildPoint()
    {
        rend.material.color = startColor;
    }

    // Returns radius of build point
    public float GetRadius()
    {
        return radius;
    }

}
