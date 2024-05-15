using UnityEngine;

public class CannonProjectile : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    
    private float damage;
    private GameObject impactEffet;
    private Transform target;
    private Rigidbody rb;
    private float peakHeight;
    private float gravity = 9.81f;
    private float explosionRadius;
    private Tower attackingTower;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (transform.position.y <= target.position.y)
        {
            HitTarget();
            return;
        }
    }

    // Launches the projectile
    void Launch()
    {
        Vector3 displacement = target.position - transform.position;

        // Calculate time to reach target position
        float timeToTarget = Mathf.Sqrt(2 * peakHeight / gravity) + Mathf.Sqrt(2 * peakHeight / gravity);

        // Calculate initial velocity components
        float velocityX = displacement.x / timeToTarget;
        float velocityY = Mathf.Sqrt(2 * gravity * peakHeight);
        float velocityZ = displacement.z / timeToTarget;

        // Apply initial velocity
        Vector3 launchVelocity = new Vector3(velocityX, velocityY, velocityZ);
        rb.velocity = launchVelocity;
    }

    // Hits target
    private void HitTarget()
    {
        Collider[] intersectingColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider col in intersectingColliders)
        {
            if (col.CompareTag(enemyTag))
            {
                Enemy enemy = col.GetComponent<Enemy>();
                enemy.TakeDamage(damage, attackingTower);
            }

        }

        GameObject impactEffectGO = Instantiate(impactEffet, transform.position, transform.rotation);
        impactEffectGO.transform.localScale = new Vector3(3f, 3f, 3f);
        Destroy(impactEffectGO, 2f);
        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // Projectile constructor
    public void ConstructProjectile(Transform target, float damage, GameObject impactEffet, float explosionRadius, Tower attackingTower)
    {
        this.target = target;
        this.damage = damage;
        this.impactEffet = impactEffet;
        this.explosionRadius = explosionRadius;
        this.attackingTower = attackingTower;

        peakHeight = transform.position.y + target.position.y / 1.5f;

        rb = GetComponent<Rigidbody>();
        Launch();

    }

}
