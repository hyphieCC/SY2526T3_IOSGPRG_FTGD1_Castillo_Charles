using UnityEngine;

namespace Castillo.Weapons
{
    public class Shotgun : WeaponBase
    {
        [Header("Shotgun Specs")]
        [SerializeField] private int _pelletCount = 8;
        [SerializeField] private float _spreadAngle = 20f;

        protected override void FireProjectile()
        {
            for (int i = 0; i < _pelletCount; i++)
            {
                FirePellet();
            }
        }

        private void FirePellet()
        {
            float randomAngle = Random.Range(
                -_spreadAngle * 0.5f,
                _spreadAngle * 0.5f
            );

            Quaternion pelletRotation =
                _firePoint.rotation *
                Quaternion.Euler(0f, 0f, randomAngle);

            Projectile projectile = Instantiate(
                _projectilePrefab,
                _firePoint.position,
                pelletRotation
            );

            projectile.Launch(
                pelletRotation * Vector2.up,
                Owner
            );
        }
    }
}