using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Fireball : Projectile
    {
        [SerializeField] private GameObject hitVFX;

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HealthHandler target))
            {
                if (target == _target)
                    target.TakeDamage(_damage);
            }

            _isHit = true;
            hitVFX.SetActive(true);
            Destroy(gameObject, 2f);
        }
    }
}