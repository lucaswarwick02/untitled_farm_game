using System;
using System.Linq;
using Data;
using ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        private Interactable _currentInteractable;

        private const float InteractRadius = 1f;
    
        public int CurrentIndex { get; private set; }
    
        public static event Action OnSlotChange;
        
        public ItemInstance CurrentItem => SaveManager.CurrentSave.inventory.equipped[CurrentIndex];

        public void NextSlot(InputAction.CallbackContext _)
        {
            CurrentIndex = (CurrentIndex + 1) % 9;
            OnSlotChange?.Invoke();
        }

        public void PreviousSlot(InputAction.CallbackContext _)
        {
            CurrentIndex = (CurrentIndex - 1 + 9) % 9;
            OnSlotChange?.Invoke();
        }

        public void Interact(InputAction.CallbackContext _)
        {
            if (_currentInteractable == null) return;
        
            _currentInteractable.onInteract.Invoke();
        }

        private void Update()
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, InteractRadius);

            colliders = colliders.Where(c => c.GetComponent<Interactable>() != null).ToArray();
            colliders = colliders.Where(c => c.GetComponent<Interactable>().enabled).ToArray();

            if (colliders.Length == 0)
            {
                // Found no colliders
                if (_currentInteractable == null) return;
                // Previous was not none, so we need to exit the previous
                _currentInteractable.onExit.Invoke();
                _currentInteractable = null;
                return;
            }
            
            var minDistance = Mathf.Infinity;
            Interactable closestInteractable = null;

            foreach (var foundCollider in colliders)
            {
                if (!(Vector2.Distance(transform.position, foundCollider.transform.position) < minDistance)) continue;
                
                // New closest collider
                closestInteractable = foundCollider.GetComponent<Interactable>();
                minDistance = Vector2.Distance(transform.position, foundCollider.transform.position);
            }
            
            // closestInteractable is now the closest interactable we have found

            if (!_currentInteractable)
            {
                // Previous was none, so this must be a new one
                _currentInteractable = closestInteractable;
                _currentInteractable!.onEnter.Invoke();
            }
            else
            {
                // Previous was not none, so we need to check if it's the same as the new one
                if (_currentInteractable == closestInteractable) return;
                
                // Not the same, so we need to exit the previous and enter the new one
                _currentInteractable.onExit.Invoke();
                _currentInteractable = closestInteractable;
                _currentInteractable!.onEnter.Invoke();
            }
        }
    }
}