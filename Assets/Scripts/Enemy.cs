using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float health = 100f;
    [SerializeField] private int damage = 1;
    [SerializeField] private string playerTag = "Player";

    private Transform target;
    private int pathElementIdx = 0;

    private Animator animator;
    public bool IsDead { get; private set; } = false;

    private void Start()
    {
        SetTarget();
        // Sets initial rotation
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);

        animator = GetComponent<Animator>();
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
        Destroy(gameObject, 2f);

        WaveSpawner.EnemiesAlive -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            Debug.Log("Enemy collided with player");
        }
    }

}
