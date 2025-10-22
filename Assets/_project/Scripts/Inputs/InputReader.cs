using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace com.torrenzo.Foundation {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, IGameplayActions {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };
        public event UnityAction<bool> Jump = delegate { };
        public event UnityAction<bool> Dash = delegate { };
        public event UnityAction Attack = delegate { };

        PlayerInputActions inputActions;

        public Vector3 Direction => inputActions.Gameplay.Move.ReadValue<Vector2>();

        void OnEnable() {
            if (inputActions == null) {
                inputActions = new PlayerInputActions();
                inputActions.Gameplay.SetCallbacks(this);
            }
        }

        void OnDisable() {
               inputActions.Gameplay.RemoveCallbacks(this);
        }

        public void EnablePlayerActions() {
            inputActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context) {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnToggleRun(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }
        public void OnInteraction(InputAction.CallbackContext context) {
            throw new System.NotImplementedException();
        }

        public void OnJump(InputAction.CallbackContext context) {
            switch (context.phase) {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }

    }
}

