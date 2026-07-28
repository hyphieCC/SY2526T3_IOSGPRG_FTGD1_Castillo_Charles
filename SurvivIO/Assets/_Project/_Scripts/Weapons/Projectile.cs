using UnityEngine;
using Castillo.Combat;

namespace Castillo.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _moveSpeed = 15f;

        [Header("Damage")]
        [SerializeField] private float _damage = 10f;

        [Header("Lifetime")]
        [SerializeField] private float _maximumLifetime = 5f;

        private Health _ownerHealth;

        private void Start()
        {
            Destroy(gameObject, _maximumLifetime);
        }

        public void Launch(Vector2 direction, GameObject owner)
        {
            _ownerHealth = owner.GetComponent<Health>();

            _rigidbody.velocity = direction.normalized * _moveSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Health hitHealth = other.GetComponentInParent<Health>();

            if (hitHealth != null && hitHealth == _ownerHealth)
            {
                return;
            }

            IDamageable damageable =
                other.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }
    }
}