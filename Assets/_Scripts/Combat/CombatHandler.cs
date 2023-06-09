using UnityEngine;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        public void Attack(CombatTarget target)
        {
            print($"Attacked to {target.name}");
        }
    }
}
