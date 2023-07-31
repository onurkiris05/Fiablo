using RPG.Interfaces;
using TMPro;
using UnityEngine;

namespace RPG.Managers
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI experienceText;
        [SerializeField] private TextMeshProUGUI levelText;

        public void OnHealthChanged(float healthPercent)
        {
            healthText.text = $"{healthPercent}%";
        }

        public void OnExperienceChanged(float experience)
        {
            experienceText.text = $"{experience}";
        }

        public void OnLevelChanged(int level)
        {
            levelText.text = $"{level}";
            Debug.Log($"Player leveled up to Level {level}");
        }
    }
}