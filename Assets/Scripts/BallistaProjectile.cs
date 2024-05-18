using UnityEngine;

public class BallistaProjectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private GameObject impactEffet;
    private Transform targetEnemy;
    private Transform targetBody;
    private Tower attackingTower;
    private Enemy enemy;

    void Update()
    {
        if (targetEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        FollowTarget();

    }
    
    // Follows target until it is hit
    private void FollowTarget()
    {
        Vector3 direction = targetBody.position - transform.position;
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
        enemy.TakeDamage(damage, attackingTower);

        Destroy(gameObject);
    }

    // Projectile constructor
    public void ConstructProjectile(Transform target, float damage, float speed, GameObject impactEffet, Tower attackingTower)
    {
        this.targetEnemy = target;
        enemy = targetEnemy.GetComponent<Enemy>();
        targetBody = enemy.GetEnemyBody();

        this.damage = damage;
        this.speed = speed;
        this.impactEffet = impactEffet;
        this.attackingTower = attackingTower;
    }

}
