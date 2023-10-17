using System;
using UnityEngine.InputSystem;

namespace UnityTools.Input
{
    public static class InputManager
    {
        private static InputActionsCore _inputActions = new();

        static InputManager()
        {
            _inputActions = null;
            _inputActions = new();
        }

        public static void AddListener(string inputActionNameOrId, Action<InputAction.CallbackContext> listener)
        {
            if (_inputActions == null)
                throw new NullReferenceException("[InputManager]: The InputManager has not been initialized yet.");

            var action = _inputActions.FindAction(inputActionNameOrId);
            if (action != null) {
                action.started += listener;
                action.performed += listener;
                action.canceled += listener;
                return;
            }
            throw new ArgumentException($"[InputManager]: The action with name/id ({inputActionNameOrId}) could not be found.");
        }

        public static void RemoveListener(string inputActionNameOrId, Action<InputAction.CallbackContext> listener)
        {
            if (_inputActions == null)
                throw new NullReferenceException("[InputManager]: The InputManager has not been initialized yet.");

            var action = _inputActions.FindAction(inputActionNameOrId);
            if (action != null) {
                action.started -= listener;
                action.performed -= listener;
                action.canceled -= listener;
                return;
            }
            throw new ArgumentException($"[InputManager]: The action with name/id ({inputActionNameOrId}) could not be found.");
        }
    }
}
