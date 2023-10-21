// MIT License
//
// Copyright (c) 2023 Max Bronstring
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace UnityTools.Input
{
    using InputListener = Action<InputAction.CallbackContext>;

    /// <summary>
    /// A static input manager class that binds methods to the input action asset.
    /// </summary>
    public static class InputManager
    {
        private static InputActionAsset[] s_inputActionAssets = null;
        private static bool s_initialized = false;
        private static List<InputListener> s_listenerQue = new();

        static InputManager()
        {
            s_initialized = false;
            s_inputActionAssets = null;
            s_listenerQue = null;
        }

        // Populate the input action assets array with an input action asset of choice.
        public static void Initialize(params InputActionAsset[] assets)
        {
            var invalidActionsException = new Exception("You are trying to initialize the Input Manager with no Input Action Asset.");

            if (assets.Length < 1)
                throw new ArgumentNullException("assets", invalidActionsException);

            if (!s_initialized) {
                s_inputActionAssets = assets.Where(asset => asset != null).ToArray();

                if (s_inputActionAssets.Length == 0)
                    throw new ArgumentNullException("s_inputActionAssets", invalidActionsException);

                for (int i = 0; i < s_inputActionAssets.Length; i++) {
                    s_inputActionAssets[i].Enable();
                }

                s_initialized = true;

                if (s_listenerQue != null) {
                    AddListener(s_listenerQue.ToArray());
                    s_listenerQue = null;
                }
            }
        }

        // Destroy all the references to the input action asset(s).
        public static void Dinitialize()
        {
            if (s_initialized) {
                for (int i = 0; i < s_inputActionAssets.Length; i++) {
                    s_inputActionAssets[i].Disable();
                }

                s_initialized = false;
                s_inputActionAssets = null;
                s_listenerQue = null;
            }
        }

        /// <summary>
        /// Add one or multiple listeners to the corresponding input action. <para />
        /// The naming format you should use for the listener is "ActionMapName + _On + ActionName". <br />
        /// (example: Player_OnJump)
        /// </summary>
        /// <param name="listeners" />
        /// <exception cref="NullReferenceException" />
        public static void AddListener(params InputListener[] listeners)
        {
            if (!s_initialized) {
                s_listenerQue ??= new();

                foreach (var listener in listeners) {
                    s_listenerQue.Add(listener);
                }
                return;
            }

            foreach (var listener in listeners) {
                // Parse the listener's name to an input action name.
                string actionName = GetParsedActionName(listener.Method.Name);

                if (actionName == string.Empty)
                    continue;

                foreach (var asset in s_inputActionAssets) {
                    // Try and find the corresponding input action.
                    var action = asset.FindAction(actionName);

                    // If the found input action is valid, the listener will
                    // subscribe to the input action.
                    if (action != null) {
                        action.started += listener;
                        action.performed += listener;
                        action.canceled += listener;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Remove one or multiple listeners from the corresponding input action. <para />
        /// The naming format you should use for the listener is "ActionMapName + _On + ActionName". <br />
        /// (example: Player_OnJump)
        /// </summary>
        /// <param name="listeners" />
        /// <exception cref="NullReferenceException" />
        public static void RemoveListener(params InputListener[] listeners)
        {
            if (!s_initialized) {
                if (s_listenerQue.Count < 1)
                    return;

                foreach (var listener in listeners) {
                    s_listenerQue.Remove(listener);
                }
                return;
            }

            foreach (var asset in s_inputActionAssets) {
                foreach (var listener in listeners) {

                    // Parse the listener's name to an input action name and
                    // try and find the corresponding input action.
                    var actionName = GetParsedActionName(listener.Method.Name);
                    var action = asset.FindAction(actionName);

                    // If the found input action is valid, the listener will
                    // unsubscribe from the input action.
                    if (action != null) {
                        action.started -= listener;
                        action.performed -= listener;
                        action.canceled -= listener;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Enable a specific action in the active input action asset(s).
        /// </summary>
        /// <param name="action" />
        public static void Enable(InputListener action)
        {
            var actionName = GetParsedActionName(action.Method.Name);
            foreach (var asset in s_inputActionAssets) {
                asset.FindAction(actionName, true)?.Enable();
            }
        }

        /// <summary>
        /// Disable a specific action in the active input action asset(s).
        /// </summary>
        /// <param name="action" />
        public static void Disable(InputListener action)
        {
            var actionName = GetParsedActionName(action.Method.Name);
            foreach (var asset in s_inputActionAssets) {
                asset.FindAction(actionName, true)?.Disable();
            }
        }

        /// <summary>
        /// Enable a specific action map in the input action asset(s).
        /// </summary>
        /// <param name="actionMap" />
        public static void EnableActionMap(string actionMap)
        {
            foreach (var asset in s_inputActionAssets) {
                asset.FindActionMap(actionMap, true)?.Enable();
            }
        }

        /// <summary>
        /// Disable a specific action map in the input action asset(s).
        /// </summary>
        /// <param name="actionMap" />
        public static void DisableActionMap(string actionMap)
        {
            foreach (var asset in s_inputActionAssets) {
                asset.FindActionMap(actionMap, true)?.Disable();
            }
        }

        /// <summary>
        /// Parse the method name to an input action name. <br />
        /// This converts "ActionMap_OnInputAction" to "ActionMap/InputAction"
        /// </summary>
        /// <param name="methodName" />
        /// <returns />
        private static string GetParsedActionName(string methodName)
        {
            return methodName.Replace("_On", "/");
        }
    }
}
