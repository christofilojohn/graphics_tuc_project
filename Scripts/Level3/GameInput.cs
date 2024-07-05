using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3.scripts
{
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnInteractAction;
        private PlayerInputActions _playerInputActions;
        private bool _swingSword;
        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Interact.performed += Interact_performed;
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (OnInteractAction != null)
            {
                OnInteractAction(this, EventArgs.Empty);
            }
        
            //OnInteractAction?.invoke(this, EventArgs.Empty);
 
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVetor = _playerInputActions.Player.Move.ReadValue<Vector2>();
            inputVetor = inputVetor.normalized;
            return inputVetor;
        }
        
    }

}