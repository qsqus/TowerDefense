using UnityEngine;

public class BlasterTower : Tower
{
    [SerializeField] private int bulletsInBurst = 3;
    [SerializeField] private float burstFireRate = 5f;    // How often bullets are shot during burst
    [SerializeField] private Transform firePoint2;

    private Transform currentFirepoint = null;
    private int bulletCounter = 0;

    protected override void SetRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(rotationPoint.eulerAngles.x, rotation.y, 0f);
    }

    protected override void HandleShooting()
    {

        if (fireCountdown <= 0f)
        {
            Shoot();
            if (bulletCounter < bulletsInBurst)
            {
                bulletCounter += 1;
                fireCountdown = 1f / burstFireRate;
            }
            else
            {
                bulletCounter = 0;
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    protected override void Shoot()
    {
        // Switch between firepoints each shot
        if(currentFirepoint == firePoint)
        {
            currentFirepoint = firePoint2;
        }
        else 
        {
            currentFirepoint = firePoint;
        }

        InstantiateShootEffect(currentFirepoint);

        GameObject projectileGO = Instantiate(projectile, currentFirepoint.position, rotationPoint.rotation);
        // Used BallistaProjectilecode for BlasterProjectile prefab
        BallistaProjectile firedProjectile = projectileGO.GetComponent<BallistaProjectile>();

        if (firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileSpeed, projectileImpactEffet, this);
        }
    }

}
