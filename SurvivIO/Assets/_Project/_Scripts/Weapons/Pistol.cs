using UnityEngine;

namespace Castillo.Weapons
{
    public class Pistol : WeaponBase
    {
        protected override void FireProjectile()
        {
            Projectile projectile = Instantiate(
                _projectilePrefab,
                _firePoint.position,
                _firePoint.rotation
            );

            projectile.Launch(_firePoint.up);
        }
    }
}