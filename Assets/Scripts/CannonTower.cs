using UnityEngine;

public class CannonTower : Tower
{
    [Header("Projectile stats")]
    [SerializeField] private float projectileExplosionRadius = 5f;


    // Sets rotation to target - locks on the target
    protected override void SetRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(rotationPoint.eulerAngles.x, rotation.y, 0f);
    }

    // Shoots target by instantiating projectile
    protected override void Shoot()
    {
        GameObject projectileGO = Instantiate(projectile, firePoint.position, rotationPoint.rotation);
        CannonProjectile firedProjectile = projectileGO.GetComponent<CannonProjectile>();

        if(firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileImpactEffet, projectileExplosionRadius);
        }
    }

}
