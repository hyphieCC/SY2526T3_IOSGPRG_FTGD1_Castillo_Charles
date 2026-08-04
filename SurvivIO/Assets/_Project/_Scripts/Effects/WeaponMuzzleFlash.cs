using System.Collections;
using UnityEngine;
using Castillo.Weapons;

namespace Castillo.Effects
{
    public class WeaponMuzzleFlash : MonoBehaviour
    {
        [SerializeField] private WeaponBase _weapon;
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private float _flashDuration = 0.05f;

        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _muzzleFlash.SetActive(false);
        }

        private void OnEnable()
        {
            _weapon.Fired += ShowMuzzleFlash;
        }

        private void OnDisable()
        {
            _weapon.Fired -= ShowMuzzleFlash;

            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                _flashCoroutine = null;
            }

            _muzzleFlash.SetActive(false);
        }

        private void ShowMuzzleFlash(WeaponBase weapon)
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }

            _flashCoroutine = StartCoroutine(CO_ShowMuzzleFlash());
        }

        private IEnumerator CO_ShowMuzzleFlash()
        {
            _muzzleFlash.SetActive(true);

            yield return new WaitForSeconds(_flashDuration);

            _muzzleFlash.SetActive(false);
            _flashCoroutine = null;
        }
    }
}