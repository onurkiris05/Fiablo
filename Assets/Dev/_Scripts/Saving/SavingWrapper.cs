using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] SavingSystem savingSystem;

        private const string _defaultSaveFile = "save";

        private void Start()
        {
            savingSystem.Load(_defaultSaveFile);
        }

        private void Update()
        {
            if (Keyboard.current[Key.L].wasPressedThisFrame)
            {
                savingSystem.Load(_defaultSaveFile);
                print("Loaded");
            }
            else if (Keyboard.current[Key.S].wasPressedThisFrame)
            {
                savingSystem.Save(_defaultSaveFile);
                print("Saved");
            }
        }

        [Button]
        private void DeleteSave() => savingSystem.DeleteSaveFile(_defaultSaveFile);
    }
}