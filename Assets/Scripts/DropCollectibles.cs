using System.Collections;
using UnityEngine;

public class DropCollectibles : MonoBehaviour
{
    [Header("Drop stats")]
    [SerializeField] private float collectibleMoveDuration = 0.5f;
    [SerializeField] private float maxCoinDropHeight = 5f;
    [SerializeField] private float minCoinDropHeight = 10f;
    [SerializeField] private float minDropDistance = -3f;
    [SerializeField] private float maxDropDistance = 4f;

    [Header("Collectible types")]
    public GameObject coin;
    public GameObject diamond;

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
        Vector3 endPosition = startPosition + new Vector3(Random.Range(minDropDistance, maxDropDistance), 0, Random.Range(minDropDistance, maxDropDistance)); // Random end position
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
