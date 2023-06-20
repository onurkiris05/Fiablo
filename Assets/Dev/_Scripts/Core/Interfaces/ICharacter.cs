namespace RPG.Core
{
    public interface ICharacter
    {
        bool IsDead();
        void ProcessDie();
        void ProcessSetTrigger(string triggerName);
        void ProcessResetTrigger(string triggerName);
    }
}