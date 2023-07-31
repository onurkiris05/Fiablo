using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 99)] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;

        private PlayerController _playerController;

        public void Init(PlayerController playerController) => _playerController = playerController;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private int GetLevel()
        {
            if (_playerController == null) return startingLevel;
            return _playerController.GetLevel();
        }
    }
}