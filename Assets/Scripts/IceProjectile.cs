using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private EnemyType[] targetedEnemyTypes = new EnemyType[] { EnemyType.Ground };

    private float slowDownFactor;
    private float slowDownDuration;


    private float speed;
    private float explosionRadius;
    private GameObject impactEffect;
    private Transform target;
    private Tower attackingTower;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = Time.deltaTime * speed;    // Amount of distance traveled in current frame

        // direction.magnitude is current distance to target
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
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

                for (int i = 0; i < targetedEnemyTypes.Length; i++)
                {
                    if (targetedEnemyTypes[i] == enemy.GetEnemyType())
                    {
                        enemy.SlowDownEnemy(slowDownFactor, slowDownDuration);
                        break;
                    }
                }
            }

        }

        //GameObject impactEffectGO = Instantiate(attackEffect, transform.position, transform.rotation);
        //float towerRange = attackingTower.GetTowerRange() * 0.7f;
        //impactEffectGO.transform.localScale = new Vector3(towerRange, towerRange * 0.7f, towerRange);

        //Destroy(impactEffectGO, 2f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingTower.GetTowerRange());
    }

    // Projectile constructor
    public void ConstructProjectile(Transform target, float slowDownFactor, float slowDownDuration, float speed, GameObject impactEffet, float explosionRadius, Tower attackingTower)
    {
        this.target = target;
        this.slowDownFactor = slowDownFactor;
        this.slowDownDuration = slowDownDuration;
        this.speed = speed;
        this.impactEffect = impactEffet;
        this.explosionRadius = explosionRadius;
        this.attackingTower = attackingTower;

    }
}
