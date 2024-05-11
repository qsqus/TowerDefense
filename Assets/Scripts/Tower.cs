using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Tower stats")]
    [SerializeField] private float range = 10f;
    [SerializeField] protected float damage = 5f;
    [SerializeField] private float fireRate = 1f;    // 1 projectile gets fired every 1/fireRate seconds
    [SerializeField] protected float projectileSpeed = 30f;
    [SerializeField] protected float rotationSpeed = 7f;
    [SerializeField] protected int price = 100;

    [Header("Tags")]
    [SerializeField] private string enemyTag = "Enemy";

    [Header("References")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] protected Transform rotationPoint;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected GameObject projectileImpactEffet;

    protected Transform target;
    protected float fireCountdown = 0f;


    void Start()
    {
        // Calls Update Target every 0.2 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }
    
    // Updates/sets nearest enemy in range as target every 0.2 seconds
    private void UpdateTarget()
    {
        if (target != null && target.GetComponent<Enemy>().IsDead)
        {
            target = null;
        }

        // Does not change target unless has to
        if (target != null && range >= Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)))
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().IsDead)
            {
                continue;
            }

            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(enemy.transform.position.x, enemy.transform.position.z));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Draws a gizmo sphere visual
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Returns renderers
    public Renderer[] GetRenderers()
    {
        return renderers;
    }

    void Update()
    {
        // If no target do nothing
        if (target == null)
        {
            return;
        }

        SetRotation();

        HandleShooting();

    }
    // Handles shooting
    private void HandleShooting()
    {
        // Prevents shooting before tower is looking in enemy direction
        /*
        if (Mathf.Abs((Quaternion.LookRotation(target.position - transform.position).eulerAngles - rotationPoint.transform.eulerAngles).x) > 2)
        {
            Debug.Log("TOO EARLY");
        }
        */

        if (fireCountdown <= 0f)
        {
            Shoot();
            // if fire rate is 2 then that means we want to fire 2 bullets each second that means it should countdown from 0.5
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    public int GetPrice()
    {
        return price;
    }
    protected abstract void SetRotation();

    protected abstract void Shoot();

}
