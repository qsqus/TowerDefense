using System.Collections;
using UnityEngine;

public class DropCollectibles : MonoBehaviour
{
    [Header("Drop stats")]
    [SerializeField] private float collectibleMoveDuration = 0.5f;
    [SerializeField] private float maxCoinDropHeight = 5f;
    [SerializeField] private float minCoinDropHeight = 10f;
    [SerializeField] private float dropRadius = 3f;

    [Header("Tags")]
    [SerializeField] private string collectiblesTag = "Collectibles";

    private Transform collectibles;


    private void Start()
    {
        collectibles = GameObject.FindGameObjectWithTag(collectiblesTag).transform;
        
    }

    // Drops given amount of a collectible type
    public void DropAmountOfCollectibles(GameObject collectible, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject coinInstance = Instantiate(collectible, transform.position, Quaternion.Euler(Vector3.zero));
            coinInstance.transform.SetParent(collectibles);
            StartCoroutine(MoveCollectible(coinInstance));
        }

    }

    // Moves collectible
    private IEnumerator MoveCollectible(GameObject collectible)
    {
        Vector3 controlPointOffset = new Vector3(0, Random.Range(minCoinDropHeight, maxCoinDropHeight), 0);
        
        Vector3 startPosition = collectible.transform.position;

        // Calculate the x and y coordinates using polar coordinates
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Random.Range(-dropRadius, dropRadius) * Mathf.Cos(angle);
        float z = Random.Range(-dropRadius, dropRadius) * Mathf.Sin(angle);

        Vector3 endPosition = startPosition + new Vector3(x, 0, z); // Random end position
        Vector3 controlPoint = startPosition + controlPointOffset; // Calculate the control point

        float startTime = Time.time;
        while (Time.time - startTime < collectibleMoveDuration)
        {
            float t = (Time.time - startTime) / collectibleMoveDuration;

            // Calculate the position using the quadratic Bézier curve formula
            Vector3 newPosition = CalculateBezierPoint(t, startPosition, controlPoint, endPosition);
            collectible.transform.position = newPosition;

            yield return null;
        }

        // Ensure the final position is exactly the end position
        collectible.transform.position = endPosition;
        collectible.GetComponent<Collectible>().SetCanBeCollected(true);
    }


    // Calculates bezier point
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0; // Start point
        point += 2 * u * t * p1; // Control point
        point += tt * p2; // End point

        return point;
    }

}
