using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField, ReadOnly] string uniqueIdentifier = "";

        // CACHED STATE
        private static Dictionary<string, SaveableEntity> _globalLookup = new();
        private List<ISaveable> saveables = new();


        private void Awake() => saveables = GetComponents<ISaveable>().ToList();
        public string GetUniqueIdentifier() => uniqueIdentifier;

        public JToken CaptureAsJtoken()
        {
            var state = new JObject();
            IDictionary<string, JToken> stateDict = state;
            
            foreach (var saveable in saveables)
            {
                var token = saveable.CaptureAsJToken();
                var component = saveable.GetType().ToString();
                stateDict[component] = token;
                Debug.Log($"{name} Capture {component} = {token}");
            }

            return state;
        }

        public void RestoreFromJToken(JToken s)
        {
            var state = s.ToObject<JObject>();
            IDictionary<string, JToken> stateDict = state;
            
            foreach (var saveable in saveables)
            {
                var component = saveable.GetType().ToString();
                if (stateDict.ContainsKey(component))
                {
                    saveable.RestoreFromJToken(stateDict[component]);
                    Debug.Log($"{name} Restore {component} =>{stateDict[component]}");
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            _globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!_globalLookup.ContainsKey(candidate)) return true;

            if (_globalLookup[candidate] == this) return true;

            if (_globalLookup[candidate] == null)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            if (_globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}