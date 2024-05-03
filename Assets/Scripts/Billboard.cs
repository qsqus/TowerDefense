using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private string cameraTag = "MainCamera";

    private GameObject cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag(cameraTag);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
