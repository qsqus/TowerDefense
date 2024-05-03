using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float health = 100f;
    [SerializeField] private int damage = 1;
    
    [Header("Collectibles")]
    [SerializeField] private int minCoinDrop = 3;
    [SerializeField] private int maxCoinDrop = 6;
    [SerializeField] private int minDiamondDrop = 1;
    [SerializeField] private int maxDiamondDrop = 1;

    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";

    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    private Transform target;
    private int pathElementIdx = 0;

    private Animator animator;
    private DropCollectibles dropCollectibles;
    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        healthBar.SetMaxValue(health);

        SetTarget();
        // Sets initial rotation
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);

        animator = GetComponent<Animator>();
        dropCollectibles = GetComponent<DropCollectibles>();

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
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetValue(health);

        // Destroys enemy when runs out of health
        if(health <= 0 && !IsDead)
        {
            Die();
        }
    }

    // Kills enemy
    private void Die()
    {
        IsDead = true;
        animator.SetBool("isDead", true);

        int coinsAmount = Random.Range(minCoinDrop, maxCoinDrop + 1);
        int diamondsAmount = Random.Range(minDiamondDrop, maxDiamondDrop + 1);
        dropCollectibles.DropAmountOfCollectibles(dropCollectibles.coin, coinsAmount);
        dropCollectibles.DropAmountOfCollectibles(dropCollectibles.diamond, diamondsAmount);

        WaveSpawner.EnemiesAlive -= 1;

        Destroy(gameObject, 2f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag) && !IsDead)
        {
            Debug.Log("Enemy collided with player");
            other.GetComponent<PlayerMovement>().CollideWithEnemy(transform.rotation);
        }
    }

}
