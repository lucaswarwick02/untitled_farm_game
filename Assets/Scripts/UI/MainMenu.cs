using System;
using System.IO;
using Data;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI currentGuidText;
        [SerializeField] private Button selectButton;
        [SerializeField] private Button[] allButtons;

        private void OnEnable()
        {
            ToggleButtons();
            SelectTopButton();
        }

        private void ToggleButtons()
        {
            var disableContinue = true;

            // Check if there is a save to attempt to load
            if (!string.IsNullOrEmpty(ProfileManager.CurrentProfile.mostRecentSave))
            {
                try
                {
                    SaveManager.Load(ProfileManager.CurrentProfile.mostRecentSave);
                    disableContinue = false;
                }
                catch (Exception)
                {
                    ProfileManager.CurrentProfile.mostRecentSave = "";
                }
            }

            continueButton.interactable = !disableContinue;
            currentGuidText.text = disableContinue ? "" : SaveManager.CurrentSave.guid[..8];

            // Check if there are any saves to select
            selectButton.interactable = Directory.GetFiles(SaveManager.SavesFolderPath, "*.json", SearchOption.TopDirectoryOnly).Length > 0;
        }

        private void SelectTopButton()
        {
            foreach (var button in allButtons)
            {
                if (!button.interactable) continue;

                ControlScheme.RefocusTarget = button.gameObject;
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
        }

        public void Continue()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public void NewFarm()
        {
            SaveManager.NewSave();
            ToggleButtons();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}