namespace RPG.Core
{
    public interface ICharacter
    {
        void ProcessDie();
        void ProcessSetTrigger(string triggerName);
        void ProcessResetTrigger(string triggerName);
    }
}