using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible stats")]
    [SerializeField] private float moveTowardsPlayerSpeed = 10f;
    [SerializeField] private int worth  = 10;
    [SerializeField] private float maxLifeSpan = 15f;
    
    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string coinTag = "Coin";
    [SerializeField] private string diamondTag = "Diamond";

    private Transform player;
    private bool canMove = false;
    private float playerRadius;
    private float elapsedTime = 0f;
    private bool canBeCollected = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        playerRadius = player.GetComponent<CapsuleCollider>().radius;
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= maxLifeSpan)
        {
            // TO DO: Start pulsing when 3 seconds left
            Debug.Log("Collectible lifespan has ended");
            Destroy(gameObject);
            return;
        }

        if (canMove && canBeCollected)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveTowardsPlayerSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, player.position) <= playerRadius)
            {
                if(gameObject.CompareTag(coinTag))
                {
                    // Coin collected effect
                    Debug.Log("Coin collected");
                    LevelManager.instance.ChangeCoinsAmount(worth);
                }
                else if(gameObject.CompareTag(diamondTag))
                {
                    // Diamond collected effect
                    Debug.Log("Diamond collected");
                    LevelManager.instance.ChangeDiamondsAmount(worth);

                }

                Destroy(gameObject);
                return;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && other.GetComponent<PlayerMovement>().CanPlayerCollect())
        {
            canMove = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canMove = false;
        }

    }

    public void SetCanBeCollected(bool canBeCollected)
    {
        this.canBeCollected = canBeCollected;
    }
}
