using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField, ReadOnly] string uniqueIdentifier = "";

        List<ISaveable> saveables = new();

        private void Awake() => saveables = GetComponents<ISaveable>().ToList();
        public string GetUniqueIdentifier() => uniqueIdentifier;

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();

            foreach (var saveable in saveables)
                state[saveable.GetType().ToString()] = saveable.CaptureState();

            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<string, object>)state;

            foreach (var saveable in saveables)
            {
                var type = saveable.GetType().ToString();

                if (stateDict.ContainsKey(type))
                    saveable.RestoreState(stateDict[type]);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }
}