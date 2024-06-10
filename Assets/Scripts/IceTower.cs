using UnityEngine;

public class IceTower : Tower
{
    [SerializeField] private float projectileExplosionRadius = 5f;
    [SerializeField] private float slowDownFactor = 0.5f;
    [SerializeField] private float slowDownDuration = 1.5f;

    protected override void SetRotation()
    {
        Vector3 direction = targetBody.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(rotationPoint.eulerAngles.x, rotation.y, 0f);
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

    protected override void Shoot()
    {
        InstantiateShootEffect(firePoint);

        GameObject projectileGO = Instantiate(projectile, firePoint.position, rotationPoint.rotation);
        IceProjectile firedProjectile = projectileGO.GetComponent<IceProjectile>();

        if (firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, slowDownFactor, slowDownDuration, projectileSpeed, projectileImpactEffet, projectileExplosionRadius, this);
        }
    }

}
