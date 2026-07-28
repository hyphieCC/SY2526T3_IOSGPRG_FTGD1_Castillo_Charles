using UnityEngine;
using System.Collections;

namespace Castillo.Weapons
{
    public class Rifle : WeaponBase
    {
        private Coroutine _automaticFireCoroutine;

        public override void BeginFire()
        {
            if (_automaticFireCoroutine != null)
            {
                return;
            }

            _automaticFireCoroutine = StartCoroutine(CO_AutomaticFire());
        }

        public override void EndFire()
        {
            StopAutomaticFire();
        }

        protected override void FireProjectile()
        {
            Projectile projectile = Instantiate(
                _projectilePrefab,
                _firePoint.position,
                _firePoint.rotation
            );

            projectile.Launch(
                _firePoint.up,
                Owner
            );
        }

        protected override void OnDisable()
        {
            StopAutomaticFire();
            base.OnDisable();
        }

        private void StopAutomaticFire()
        {
            if (_automaticFireCoroutine == null)
            {
                return;
            }

            StopCoroutine(_automaticFireCoroutine);
            _automaticFireCoroutine = null;
        }

        private IEnumerator CO_AutomaticFire()
        {
            while (true)
            {
                TryFire();
                yield return null;
            }
        }
    }
}