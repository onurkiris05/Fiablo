using UnityEngine;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;
        
        public float WeaponRange => weaponRange;
        
        public void Attack(CombatTarget target)
        {
        }
    }
}
