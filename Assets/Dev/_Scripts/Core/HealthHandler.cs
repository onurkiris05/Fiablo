using RPG.Control;
using UnityEngine;
using Newtonsoft.Json.Linq;
using RPG.Interfaces;
using Zenject;

namespace RPG.Core
{
    public class HealthHandler : MonoBehaviour, ISaveable
    {
        [Inject] private IUIManager uiManager;
        [SerializeField] private float health;
        [SerializeField] private bool isPlayer;

        public bool IsDead => _isDead;

        private ExperienceHandler _instigator;
        private ControllerBase _character;
        private bool _isDead;
        private bool _isLoaded;

        #region ENCAPSULATIONS

        private float _health
        {
            get { return health; }
            set
            {
                health = value;
                if (isPlayer)
                {
                    var percent = (_health / _character.GetHealth()) * 100f;
                    uiManager.OnHealthChanged(percent);
                }
            }
        }

        #endregion

        private void Start()
        {
            if (!_isLoaded)
                SetHealth();
        }

        public void Init(ControllerBase character) => _character = character;

        public void SetHealth() => _health = _character.GetHealth();

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            _health = Mathf.Max(_health - damage, 0);
            print($"{transform.name} - Health left: " + _health);

            if (_health <= 0f)
            {
                RewardExperience();
                Die();
            }
        }

        public void SetInstigator(ExperienceHandler instigator) => _instigator = instigator;

        private void Die(bool isImmediate = false)
        {
            _isDead = true;
            _character.ProcessDie(isImmediate);
            print($"{gameObject.name} died!!");
        }

        private void RewardExperience()
        {
            if (isPlayer) return;
            var exp = _character.GetExperienceReward();
            _instigator.RewardExperience(exp);
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_health);
        }

        public void RestoreFromJToken(JToken state)
        {
            _health = state.ToObject<float>();
            _isLoaded = true;

            if (_health <= 0f)
                Die(true);
        }
    }
}