using UnityEngine;

public class TowerRangeDisplayManager : MonoBehaviour
{
    [SerializeField] private float borderWidth = 0.2f;
    [SerializeField] private Color borderColor;
    [SerializeField] private Color borderColorNotBuilt;
    [SerializeField] private Renderer[] groundRenderers;

    public static TowerRangeDisplayManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of TowerRangeDisplayManager in scene");

            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        HideTowerRange();
    }

    // Shows tower range in given position with given range
    public void ShowTowerRange(Vector3 towerPosition, float towerRange, bool isBuilt = true)
    {
        if(PauseMenu.IsPaused)
        {
            return;
        }

        Color color;
        if (isBuilt)
        {
            color = borderColor;
        }
        else
        {
            color = borderColorNotBuilt;
        }


        for (int i = 0; i < groundRenderers.Length; i++)
        {
            groundRenderers[i].sharedMaterial.SetVector("_Center", towerPosition);
            groundRenderers[i].sharedMaterial.SetFloat("_Border", borderWidth);
            groundRenderers[i].sharedMaterial.SetFloat("_Radius", towerRange);
            groundRenderers[i].sharedMaterial.SetFloat("_Radius", towerRange);
            groundRenderers[i].sharedMaterial.SetColor("_AreaColor", color);

        }
    }

    // Hides tower range
    public void HideTowerRange()
    {
        if (PauseMenu.IsPaused)
        {
            return;
        }

        for (int i = 0; i < groundRenderers.Length; i++)
        {
            groundRenderers[i].sharedMaterial.SetFloat("_Border", 0f);
        }
    }

}