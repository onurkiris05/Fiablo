using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab;
        [SerializeField] private Projectile projectile;
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private float weaponDamage = 15f;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        public Projectile Projectile => projectile;
        public float Damage => weaponDamage;
        public float Range => weaponRange;
        public float TimeBetweenAttacks => timeBetweenAttacks;

        public virtual void Init(Transform handTransform, ControllerBase character)
        {
            Instantiate(weaponPrefab, handTransform);
            character.SetAnimator(animatorOverride);
        }
    }
}