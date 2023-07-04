using RPG.Control;
using RPG.Saving;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace RPG.Core
{
    public class HealthHandler : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] float health = 100f;

        public bool IsDead => _isDead;

        private ControllerBase _character;
        private bool _isDead;

        public void Init(ControllerBase character)
        {
            _character = character;
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

        private void Die(bool isImmediate = false)
        {
            _isDead = true;
            _character.ProcessDie(isImmediate);
            print($"{gameObject.name} died!!");
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(health);
        }

        public void RestoreFromJToken(JToken state)
        {
            health = state.ToObject<float>();

            if (health <= 0f)
                Die(true);
        }
    }
}