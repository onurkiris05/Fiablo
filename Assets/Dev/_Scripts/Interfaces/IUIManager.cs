
namespace RPG.Interfaces
{
    public interface IUIManager
    {
        void OnHealthChanged(float healthPercent);
        void OnExperienceChanged(float experience);
        void OnLevelChanged(int level);
    }
}
