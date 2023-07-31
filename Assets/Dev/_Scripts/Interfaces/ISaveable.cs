using Newtonsoft.Json.Linq;

namespace RPG.Interfaces
{
    public interface ISaveable
    {
        JToken CaptureAsJToken();
        void RestoreFromJToken(JToken state);
    }
}