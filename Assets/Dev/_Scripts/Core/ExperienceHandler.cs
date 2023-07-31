using Newtonsoft.Json.Linq;
using RPG.Interfaces;
using RPG.Stats;
using UnityEngine;
using Zenject;

namespace RPG.Core
{
    public class ExperienceHandler : MonoBehaviour, ISaveable
    {
        [Inject] IUIManager uiManager;
        [SerializeField] private Progression progression;
        [SerializeField] private float experience;
        [SerializeField] private int level = 1;

        PlayerController _playerController;

        #region ENCAPSULATIONS

        public int Level
        {
            get { return level; }
            private set
            {
                level = value;
                uiManager.OnLevelChanged(level);
            }
        }

        private float _experience
        {
            get { return experience; }
            set
            {
                experience = value;
                uiManager.OnExperienceChanged(experience);
            }
        }

        #endregion

        public void Init(PlayerController playerController) => _playerController = playerController;

        public void RewardExperience(float amount)
        {
            _experience += amount;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            var maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, CharacterClass.Player) + 1;
            for (int level = 1; level < maxLevel; level++)
            {
                var expToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, CharacterClass.Player, level);
                if (expToLevelUp > _experience)
                {
                    if (Level < level)
                    {
                        Level = level;
                        _playerController.InvokeOnLevelUp();
                    }

                    return;
                }
            }

            if (Level < maxLevel)
            {
                Level = maxLevel;
                _playerController.InvokeOnLevelUp();
                Debug.Log("Player reached to Max level!!");
            }
        }

        public JToken CaptureAsJToken()
        {
            var data = new ExperienceData
            {
                Level = Level,
                Experience = _experience
            };
            return JToken.FromObject(data);
        }

        public void RestoreFromJToken(JToken state)
        {
            var data = state.ToObject<ExperienceData>();
            Level = data.Level;
            _experience = data.Experience;
        }

        [System.Serializable]
        class ExperienceData
        {
            public int Level;
            public float Experience;
        }
    }
}