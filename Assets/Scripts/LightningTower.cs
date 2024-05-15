using UnityEngine;

public class LightningTower : Tower
{
    protected override void SetRotation() {}

    protected override void Shoot()
    {
        GameObject projectileGO = Instantiate(projectile, firePoint.position, transform.rotation);
        LightingProjectile firedProjectile = projectileGO.GetComponent<LightingProjectile>();

        if (firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileImpactEffet, this);
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
