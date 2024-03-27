using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float range = 10f;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private float rotationSpeed = 7f;
    [SerializeField] private float fireRate = 1f;    // 1 projectile gets fired every 1/fireRate seconds
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private GameObject projectileImpactEffet;
    
    private Transform target;
    private float fireCountdown = 0f;

    void Start()
    {
        // Calls Update Target every 0.2 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    void Update()
    {
        // If no target do nothing
        if(target == null)
        {
            return;
        }

        SetRotation();

        HandleShooting();

    }

    // Updates/sets nearest enemy in range as target every 0.2 seconds
    private void UpdateTarget()
    {
        // Does not change target unless has to
        if(target != null && range >= Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)))
        {
            return;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(enemy.transform.position.x, enemy.transform.position.z));

            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy!=null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Draws a gizmo sphere visual when turret is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Sets rotation to target - locks on the target
    private void SetRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // Handles shooting
    private void HandleShooting()
    {
        if (fireCountdown <= 0f)
        {
            Shoot();
            // if fire rate is 2 then that means we want to fire 2 bullets each second that means it should countdown from 0.5
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // Shoots target by instantiating projectile
    private void Shoot()
    {
        GameObject projectileGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Projectile firedProjectile = projectileGO.GetComponent<Projectile>();

        if(firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileSpeed, projectileImpactEffet);
        }
    }
}
