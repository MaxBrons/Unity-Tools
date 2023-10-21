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

using UnityEngine;
using UnityEngine.InputSystem;
using UnityTools.Input;

namespace UnityTools.CharacterController
{
    /// <summary>
    /// A class that handles player movement input and 
    /// that interfaces with the <see cref="CharacterController2D"/>
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    public class PlayerMovement2D : MonoBehaviour
    {
        private CharacterController2D _controller;
        private Vector2 _moveDelta;
        private bool _crouching;
        private bool _jumping;

        private void Awake()
        {
            _controller = GetComponent<CharacterController2D>();
        }

        // Add all the input methods to the Input Manager.
        private void OnEnable()
        {
            InputManager.AddListener(Player_OnMove, Player_OnCrouch, Player_OnJump);
        }

        // Remove all the input methods from the Input Manager.
        private void OnDisable()
        {
            InputManager.RemoveListener(Player_OnMove, Player_OnCrouch, Player_OnJump);
        }

        // Call the character controller methods for moving, jumping and crouching.
        private void FixedUpdate()
        {
            _controller.Move(_moveDelta);
            _controller.Crouch(_crouching);

            if (_jumping) {
                _jumping = false;
                _controller.Jump();
            }
        }

        // Set the current value of the player movement input.
        private void Player_OnMove(InputAction.CallbackContext context)
        {
            if (!context.started)
                _moveDelta = context.ReadValue<Vector2>();
        }

        // Set the current value of the player crouch state.
        private void Player_OnCrouch(InputAction.CallbackContext context)
        {
            if (!context.started)
                _crouching = context.ReadValueAsButton();
        }

        // Set the current value of the player jump state.
        private void Player_OnJump(InputAction.CallbackContext context)
        {
            if (!context.started)
                _jumping = context.ReadValueAsButton();
        }
    }
}