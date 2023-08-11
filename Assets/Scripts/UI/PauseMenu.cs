using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utility;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject continueButton;
        
        private void OnEnable()
        {
            ControlScheme.RefocusTarget = continueButton;
            EventSystem.current.SetSelectedGameObject(continueButton);
        }

        public void ExitToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}