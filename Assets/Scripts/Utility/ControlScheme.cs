using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Utility
{
    [RequireComponent(typeof(PlayerInput))]
    public class ControlScheme : MonoBehaviour
    {
        private PlayerInput _playerInput;
    
        public static event Action<ControlSchemeType> OnControlSchemeChanged;

        private static ControlSchemeType CurrentControlScheme { get; set; }
    
        public static bool IsUsingKeyboard => CurrentControlScheme == ControlSchemeType.Keyboard;
        public static bool IsUsingGamepad => CurrentControlScheme == ControlSchemeType.Gamepad;
    
        public static GameObject RefocusTarget { get; set; }
    
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.onControlsChanged += OnControlsChanged;
        }
    
        private void OnDisable()
        {
            _playerInput.onControlsChanged -= OnControlsChanged;
        }

        private static void OnControlsChanged(PlayerInput playerInput)
        {
            if (playerInput.currentControlScheme.Equals("Keyboard"))
            {
                OnControlSchemeChanged?.Invoke(ControlSchemeType.Keyboard);
                CurrentControlScheme = ControlSchemeType.Keyboard;
                if (RefocusTarget && RefocusTarget.activeSelf) EventSystem.current.SetSelectedGameObject(RefocusTarget);
            }
            else if (playerInput.currentControlScheme.Equals("Gamepad"))
            {
                OnControlSchemeChanged?.Invoke(ControlSchemeType.Gamepad);
                CurrentControlScheme = ControlSchemeType.Gamepad;
                if (RefocusTarget && RefocusTarget.activeSelf) EventSystem.current.SetSelectedGameObject(RefocusTarget);
            }
            else
            {
                Debug.LogError("Unknown control scheme: " + playerInput.currentControlScheme);
            }
        }
    }

    public enum ControlSchemeType
    {
        Keyboard,
        Gamepad
    }
}