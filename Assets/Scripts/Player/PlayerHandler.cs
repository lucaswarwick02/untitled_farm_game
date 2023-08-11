using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerHandler : SingletonMonoBehaviour<PlayerHandler>
    {
        public PlayerMovement movement;
        public PlayerInteract interact;

        private GameActions _gameActions;

        private void Awake()
        {
            Instance = this;

            _gameActions = new GameActions();
        
            // Movement
            _gameActions.Player.Move.performed += movement.Move;
            _gameActions.Player.Move.canceled += movement.MoveEnd;
        
            // Interact
            _gameActions.Player.Interact.performed += interact.Interact;
            _gameActions.Player.NextSlot.performed += interact.NextSlot;
            _gameActions.Player.PreviousSlot.performed += interact.PreviousSlot;
        }

        private void OnEnable()
        {
            _gameActions.Enable();
        }
    
        private void OnDisable()
        {
            _gameActions.Disable();
        }
    }
}