using UnityEngine;

public class BallistaTower : Tower
{
    // Sets rotation to target - locks on the target
    protected override void SetRotation()
    {
        Vector3 direction = targetBody.position - firePoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        
        Vector3 rotationY = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        Vector3 rotationX = Quaternion.Lerp(xAxisRotationPoint.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        
        rotationPoint.rotation = Quaternion.Euler(0f, rotationY.y, 0f);
        xAxisRotationPoint.rotation = Quaternion.Euler(rotationX.x, rotationY.y, 0f);

    }

    // Shoots target by instantiating projectile
    protected override void Shoot()
    {
        GameObject projectileGO = Instantiate(projectile, firePoint.position, xAxisRotationPoint.rotation);
        BallistaProjectile firedProjectile = projectileGO.GetComponent<BallistaProjectile>();

        if(firedProjectile != null)
        {
            firedProjectile.ConstructProjectile(target, damage, projectileSpeed, projectileImpactEffet, this);
        }
    }

}
