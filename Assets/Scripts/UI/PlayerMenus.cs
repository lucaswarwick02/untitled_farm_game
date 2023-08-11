using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UI
{
    public class PlayerMenus : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        
        [Header("Menus")]
        [SerializeField] private GameObject hud;
        [SerializeField] private GameObject inventory;
        [SerializeField] private GameObject pause;
        
        private GameActions _gameActions;
        
        private void Awake()
        {
            _gameActions = new GameActions();
            
            _gameActions.UI.ToggleInventory.performed += _ => ToggleInventory();
            _gameActions.UI.TogglePause.performed += _ => TogglePause();
            
            hud.SetActive(true);
            inventory.SetActive(false);
            pause.SetActive(false);
        }

        private void OnEnable()
        {
            _gameActions.Enable();
        }

        private void OnDisable()
        {
            _gameActions.Disable();
        }

        public void ToggleInventory()
        {
            if (hud.activeSelf)
            {
                // HUD is active so disable and change to inventory
                hud.SetActive(false);
                inventory.SetActive(true);
                
                Time.timeScale = 0f;
                AddBlur();
            }
            else if (inventory.activeSelf)
            {
                // Inventory is active so disable and change to HUD
                inventory.SetActive(false);
                hud.SetActive(true);
                
                Time.timeScale = 1f;
                RemoveBlur();
            }
        }

        public void TogglePause()
        {
            if (hud.activeSelf)
            {
                // HUD is active so disable and change to pause
                hud.SetActive(false);
                pause.SetActive(true);
                
                Time.timeScale = 0f;
                AddBlur();
            }
            else if (pause.activeSelf)
            {
                // Pause is active so disable and change to HUD
                pause.SetActive(false);
                hud.SetActive(true);
                
                Time.timeScale = 1f;
                RemoveBlur();
            }
        }

        private void AddBlur()
        {
            volume.profile.TryGet(out DepthOfField depthOfField);
            depthOfField.active = true;
        }

        private void RemoveBlur()
        {
            volume.profile.TryGet(out DepthOfField depthOfField);
            depthOfField.active = false;
        }
    }
}
