using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileLifetime;

        protected HealthHandler _target;
        protected float _damage;
        protected bool _isHit;

        public virtual void Init(HealthHandler target, float damage)
        {
            var dir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(dir);
            _target = target;
            _damage = damage;
            
            ProcessLifetime();
        }

        protected virtual void Update()
        {
            if (_isHit) return;
            
            transform.Translate(Vector3.forward * (projectileSpeed * Time.deltaTime));
        }

        protected virtual void ProcessLifetime()
        {
            Destroy(gameObject, projectileLifetime);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HealthHandler target))
            {
                if (target == _target)
                    target.TakeDamage(_damage);
            }

            _isHit = true;
            Destroy(gameObject, 2f);
        }
    }
}