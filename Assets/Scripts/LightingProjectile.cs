using UnityEngine;

public class LightingProjectile : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private EnemyType[] targetedEnemyTypes = new EnemyType[] { EnemyType.Ground };

    private float damage;
    private GameObject attackEffect;
    private Transform target;
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


    // Hits target
    private void HitTarget()
    {
        Collider[] intersectingColliders = Physics.OverlapSphere(transform.position, attackingTower.GetTowerRange());

        foreach (Collider col in intersectingColliders)
        {
            if (col.CompareTag(enemyTag))
            {
                Enemy enemy = col.GetComponent<Enemy>();

                for (int i = 0; i < targetedEnemyTypes.Length; i++)
                {
                    if (targetedEnemyTypes[i] == enemy.GetEnemyType())
                    {
                        enemy.TakeDamage(damage, attackingTower);
                        break;
                    }
                }
            }

        }

        GameObject impactEffectGO = Instantiate(attackEffect, transform.position, transform.rotation);
        impactEffectGO.transform.localScale = new Vector3(attackingTower.GetTowerRange(), 1f, attackingTower.GetTowerRange());
        Destroy(impactEffectGO, 2f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingTower.GetTowerRange());
    }

    // Projectile constructor
    public void ConstructProjectile(Transform target, float damage, GameObject attackEffect, Tower attackingTower)
    {
        this.target = target;
        this.damage = damage;
        this.attackEffect = attackEffect;
        this.attackingTower = attackingTower;

    }
}
