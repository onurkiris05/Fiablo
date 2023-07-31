using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClasses;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = _lookupTable[characterClass][stat];
            if (levels.Length < level)
                return 0f;

            return levels[level - 1];
        }
        
        public int GetLevels(Stat stat, CharacterClass  characterClass)
        {
            BuildLookup();

            float[] levels = _lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (var progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (var progressionStat in progressionClass.Stats)
                {
                    statLookupTable[progressionStat.Stat] = progressionStat.Levels;
                }

                _lookupTable[progressionClass.CharacterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass CharacterClass;
            public ProgressionStat[] Stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat Stat;
            public float[] Levels;
        }
    }
}