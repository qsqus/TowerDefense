using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float health = 100f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float towerExperienceAmount = 0.4f;
    [SerializeField] private float afterDeathSinkDepth = 2f;
    [SerializeField] private float sinkingDuration = 8f;
    [SerializeField] private float animatorSpeed = 1f;
    
    [Header("Collectibles")]
    [SerializeField] private int minCoinDrop = 3;
    [SerializeField] private int maxCoinDrop = 6;
    [SerializeField] private int minDiamondDrop = 1;
    [SerializeField] private int maxDiamondDrop = 1;
    [SerializeField] private float chanceOfDiamondDrop = 0.15f;

    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";

    [Header("References")]
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private Animator animator;

    private Transform target;
    private int pathElementIdx = 0;

    private DropCollectibles dropCollectibles;
    public bool IsDead { get; private set; } = false;


    private void Awake()
    {
        dropCollectibles = GetComponent<DropCollectibles>();
    }

    private void Start()
    {
        healthBar.SetMaxValue(health);

        SetTarget();
        // Sets initial rotation
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        
        animator.speed = animatorSpeed;
    }

    private void Update()
    {
        if(!IsDead)
        {
            Move();
        }
    }

    // Moves enemy
    private void Move()
    {
        // Destroys enemy when there is no pathElements left
        if(target == null)
        {
            WaveSpawner.EnemiesAlive -= 1;
            LevelManager.instance.ChangeLivesByAmount(-1);
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;

        // Rotates towards target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Moves towards target
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

        // Chooses next target when arrived at current target
        if (Vector3.Distance(transform.position, target.position) <= 0.5f)
        {
            pathElementIdx += 1;
            SetTarget();
        }
    }

    // Sets target
    private void SetTarget()
    {
        target = EnemyPath.GetEnemyPathElement(pathElementIdx);
    }

    // Takes damage
    public void TakeDamage(float amount, Tower attackingTower)
    {
        health -= amount;
        healthBar.SetValue(health);

        // Destroys enemy when runs out of health
        if(health <= 0 && !IsDead)
        {
            Die(attackingTower);
        }
    }

    // Kills enemy
    private void Die(Tower attackingTower)
    {
        attackingTower.AddUpgradeProgress(towerExperienceAmount);

        IsDead = true;
        animator.SetBool("isDead", true);

        int coinsAmount = Random.Range(minCoinDrop, maxCoinDrop + 1);
        dropCollectibles.DropAmountOfCollectibles(LevelManager.instance.coin, coinsAmount);
        
        bool isDiamondDrop = (Random.Range(0f, 1f) <= chanceOfDiamondDrop) ? true : false;
        if(isDiamondDrop)
        {
            int diamondsAmount = Random.Range(minDiamondDrop, maxDiamondDrop + 1);
            dropCollectibles.DropAmountOfCollectibles(LevelManager.instance.diamond, diamondsAmount);
        }

        WaveSpawner.EnemiesAlive -= 1;

        //Destroy(gameObject, 2f);
        StartCoroutine(SinkUnderground(sinkingDuration));

    }

    private IEnumerator SinkUnderground(float sinkingDuration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position - new Vector3(0, afterDeathSinkDepth, 0);
        float elapsedTime = 0f;

        while(elapsedTime < sinkingDuration)
        {
            float t = elapsedTime / sinkingDuration;
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = targetPosition;

        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag) && !IsDead)
        {
            Vector3 direction = other.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(Vector3.Dot(direction, transform.right) < 0)
            {
                other.GetComponent<PlayerMovement>().CollideWithEnemy(-transform.right);
            }
            else
            {
                other.GetComponent<PlayerMovement>().CollideWithEnemy(transform.right);
            }
        }
    }

}
