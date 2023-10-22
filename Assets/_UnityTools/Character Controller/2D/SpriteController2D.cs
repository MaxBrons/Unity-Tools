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
    /// A class that handles the swapping of sprites from a given list of 
    /// sprites, based on a set condition.
    /// </summary>
    public class SpriteController2D : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private bool _useMousePosition = true;

        private Vector2 _currentPointerPosition;
        private Vector2 _moveDelta;

        // Check the set condition and change the current sprite 
        // to the condition's result.
        private void FixedUpdate()
        {
            if (_useMousePosition) {
                var worldPos = Camera.main.ScreenToWorldPoint(_currentPointerPosition);
                var angle = Vector3.SignedAngle(worldPos, transform.position, transform.up);

                _spriteRenderer.flipX = angle < 0;
                return;
            }

            if (_moveDelta.x != 0)
                _spriteRenderer.flipX = _moveDelta.x < 0;
        }

        // Add all the input methods to the Input Manager.
        private void OnEnable()
        {
            InputManager.AddListener(Player_OnPoint, Player_OnMove);
        }

        // Remove all the input methods from the Input Manager.
        private void OnDisable()
        {
            InputManager.RemoveListener(Player_OnPoint, Player_OnMove);
        }

        // Set the current value of the pointer position on screen.
        private void Player_OnPoint(InputAction.CallbackContext context)
        {
            if (context.performed)
                _currentPointerPosition = context.ReadValue<Vector2>();
        }

        // Set the current value of the player movement input.
        private void Player_OnMove(InputAction.CallbackContext context)
        {
            if (!context.started)
                _moveDelta = context.ReadValue<Vector2>();
        }
    }
}