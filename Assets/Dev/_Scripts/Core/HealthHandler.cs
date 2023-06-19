using UnityEngine;

namespace RPG.Core
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField ] float health = 100f;
        
        private bool _isDead;

        public void TakeDamage(float damage)
        {
            if(_isDead) return;

            health = Mathf.Max(health - damage, 0);
            if (health <= 0f)
            {
                Die();
                return;
            }
            
            print("Health left: " + health);
        }

        private void Die()
        {
            _isDead = true;
            print("Died!!");
        }
    }
}
