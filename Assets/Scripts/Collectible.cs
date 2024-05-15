using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible stats")]
    [SerializeField] private float moveTowardsPlayerSpeed = 10f;
    [SerializeField] private int worth  = 10;
    [SerializeField] private float maxLifeSpan = 15f;
    [SerializeField] private float flashWhenTimeLeft = 3f;

    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string coinTag = "Coin";
    [SerializeField] private string diamondTag = "Diamond";

    [Header("References")]
    [SerializeField] private MaterialFlash materialFlash;

    private Transform player;
    private bool canMove = false;
    private float playerRadius;
    private float elapsedTime = 0f;
    private bool canBeCollected = false;
    private bool hasFlashingStarted = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        playerRadius = player.GetComponent<CapsuleCollider>().radius;
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Colletible lifespan has ended
        if(materialFlash.HasFinishedFlashing())
        {
            Destroy(gameObject);
            return;
        }

        if(elapsedTime >= maxLifeSpan - flashWhenTimeLeft && !hasFlashingStarted)
        {
            hasFlashingStarted = true;
            materialFlash.StartFlashing(flashWhenTimeLeft);
            return;
        }

        if (canMove && canBeCollected)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveTowardsPlayerSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, player.position) <= playerRadius)
            {
                if(gameObject.CompareTag(coinTag))
                {
                    // TO DO: Add coin collected effect
                    LevelManager.instance.ChangeCoinsByAmount(worth);
                }
                else if(gameObject.CompareTag(diamondTag))
                {
                    // TO DO: Add diamond collected effect
                    LevelManager.instance.ChangeDiamondsByAmount(worth);

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

    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    public void SetCanBeCollected(bool canBeCollected)
    {
        this.canBeCollected = canBeCollected;
    }

    public int GetWorth()
    {
        return worth;
    }

}
