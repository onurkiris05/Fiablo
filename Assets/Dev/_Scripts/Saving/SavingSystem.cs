using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        [SerializeField] SavingStrategy strategy;

        public IEnumerator LoadLastScene(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            IDictionary<string, JToken> stateDict = state;
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            if (stateDict.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)stateDict["lastSceneBuildIndex"];
            }

            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreFromToken(state);
        }

        public void Save(string saveFile)
        {
            JObject state = LoadJsonFromFile(saveFile);
            CaptureAsToken(state);
            SaveFileAsJSon(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreFromToken(LoadJsonFromFile(saveFile));
        }

        public void DeleteSaveFile(string saveFile)
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path)) return;

            print($"Save file: {saveFile} deleted");
            File.Delete(path);
        }

        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == strategy.GetExtension())
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private void SaveFileAsJSon(string saveFile, JObject state)
        {
            strategy.SaveToFile(saveFile, state);
        }

        private JObject LoadJsonFromFile(string saveFile)
        {
            return strategy.LoadFromFile(saveFile);
        }

        private void CaptureAsToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (var saveable in SavingWrapper.SaveableEntities)
            {
                stateDict[saveable.GetUniqueIdentifier()] = saveable.CaptureAsJToken();
            }

            stateDict["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreFromToken(JObject state)
        {
            IDictionary<string, JToken> stateDict = state;
            foreach (var saveable in SavingWrapper.SaveableEntities)
            {
                var id = saveable.GetUniqueIdentifier();
                if (stateDict.ContainsKey(id))
                    saveable.RestoreFromJToken(stateDict[id]);
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return strategy.GetPathFromSaveFile(saveFile);
        }
    }
}