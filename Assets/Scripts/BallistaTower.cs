using UnityEngine;

public class BallistaTower : Tower
{

    // Sets rotation to target - locks on the target
    protected override void SetRotation()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }

    // Shoots target by instantiating projectile
    protected override void Shoot()
    {
        GameObject projectileGO = Instantiate(projectile, firePoint.position, rotationPoint.rotation);
        BallistaProjectile firedProjectile = projectileGO.GetComponent<BallistaProjectile>();

        if(firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileSpeed, projectileImpactEffet);
        }
    }

}
