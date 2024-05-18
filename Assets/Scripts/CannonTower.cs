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
        InstantiateShootEffect(firePoint);

        GameObject projectileGO = Instantiate(projectile, firePoint.position, rotationPoint.rotation);
        CannonProjectile firedProjectile = projectileGO.GetComponent<CannonProjectile>();

        if(firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileImpactEffet, projectileExplosionRadius, this);
        }
    }


    protected override void HandleShooting()
    {

        if (fireCountdown <= 0f)
        {
            Shoot();
            // if fire rate is 2 then that means we want to fire 2 bullets each second that means it should countdown from 0.5
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

}
