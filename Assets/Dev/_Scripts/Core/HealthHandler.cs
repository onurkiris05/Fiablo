using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        public bool IsDead => _isDead;

        private ControllerBase _character;
        private ActionScheduler _actionScheduler;
        private bool _isDead;

        public void Init(ControllerBase character, ActionScheduler scheduler)
        {
            _character = character;
            _actionScheduler = scheduler;
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

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
            _character.ProcessDie();
            _actionScheduler.CancelCurrentAction();
            print("Died!!");
        }
    }
}