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

using System.Collections.Generic;
using UnityEngine;

namespace UnityTools.CharacterController
{
    /// <summary>
    /// A physics-based 2D character controller class that handles 
    /// player movement, jumping and crouching.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        // Used for general 2D movement
        [Header("2D General/Top-Down")]
        [Tooltip("The max speed at wich the player travels.")]
        [SerializeField] private float _movementSpeed = 10f;
        [Tooltip("The smoothing that is aplied when moving.")]
        [SerializeField] private float _movementSmoothing = .05f;
        [Tooltip("The amount that the movement is multiplied by when crouching.")]
        [Range(0, 1)][SerializeField] private float _crouchSpeed = .36f;

        // Used for a 2D platformer style movement where you use gravity.             
        [Header("2D Platformer")]
        [Tooltip("Used to pick between platformer or top-down mode.")]
        [SerializeField] private bool _useGravity;
        [Tooltip("The force that will be aplied to the player when jumping.")]
        [SerializeField] private float _jumpForce = 400f;
        [Tooltip("The layer that is used to check if the player is grounded.")]
        [SerializeField] private LayerMask _groundLayer;
        [Tooltip("The location that is used to check if the player is under an obstacle")]
        [SerializeField] private Transform _ceilingCheckLocation;
        [Tooltip("The location that is used to check if the player is grounded.")]
        [SerializeField] private Transform _groundCheckLocation;
        [Tooltip("The radius that is used to check if the player is under an obstacle.")]
        [SerializeField] private float _ceilingCheckRadius = .2f;
        [Tooltip("The radius that is used to check if the player is grounded.")]
        [SerializeField] private float _groundCheckRadius = .2f;
        [Tooltip("A list of colliders that is enabled or disabled when the player crouches.")]
        [SerializeField] private List<Collider2D> _colliderToDisableOnCrouch = new();

        public Vector2 Velocity => _rb.velocity;
        public bool UseGravity => !_rb.isKinematic;

        private Vector2 _velocity = Vector2.zero;
        private Rigidbody2D _rb;
        private bool _crouching = false;
        private bool _isGrounded = false;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            SetUseGravity(_useGravity);
        }

        private void FixedUpdate()
        {
            // If the gravity is chaged through the editor or the rigidbody
            // kinematic setting, the gravity will be set to the useGravity value.
            if (_useGravity != UseGravity)
                SetUseGravity(_useGravity);

            if (!_useGravity)
                return;

            _isGrounded = false;

            // Check if there are any objects under the player with the
            // given ground layer to check if the player is grounded or not.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheckLocation.position, _groundCheckRadius, _groundLayer);
            foreach (var collider in colliders) {
                if (collider != gameObject)
                    _isGrounded = true;
            }
        }

        /// <summary>
        /// Let the player crouch or stand up, but only
        /// if there are no obstacles above the player's head.
        /// </summary>
        /// <param name="value"></param>
        public void Crouch(bool value)
        {
            _crouching = value;

            if (_crouching || !_useGravity)
                return;

            if (Physics2D.OverlapCircle(_ceilingCheckLocation.position, _ceilingCheckRadius, _groundLayer)) {
                _crouching = true;
            }
        }

        /// <summary>
        /// Add a jump force to the rigidbody when the player is grounded.
        /// </summary>
        public void Jump()
        {
            if (!_useGravity)
                return;

            if (_isGrounded) {
                _isGrounded = false;
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }

        /// <summary>
        /// Move the player in the given direction based on the set 
        /// speed and movement smoothing.
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector2 direction)
        {
            if (_crouching) {
                direction *= _crouchSpeed;
                _colliderToDisableOnCrouch?.ForEach(x => x.enabled = false);
            }
            else {
                _colliderToDisableOnCrouch?.ForEach(x => x.enabled = true);
            }

            if (_useGravity) {
                direction.y = 0;
            }

            _rb.velocity = Vector2.SmoothDamp(_rb.velocity, _movementSpeed * 10.0f * Time.fixedDeltaTime * direction, ref _velocity, _movementSmoothing);
        }

        /// <summary>
        /// Manually set the rigidbody's velocity.
        /// </summary>
        /// <param name="value"></param>
        public void SetVelocity(Vector2 value)
        {
            _rb.velocity = value;
            _velocity = _rb.velocity;
        }

        /// <summary>
        /// Set if you want the player to experience gravity during gameplay.
        /// </summary>
        /// <param name="value"></param>
        private void SetUseGravity(bool value)
        {
            _useGravity = value;
            _rb.isKinematic = !value;
            _rb.freezeRotation = value;
        }

        // Draw the gizmos for the ground and ceiling checks
        // for easy visualization in the editor.
        private void OnDrawGizmos()
        {
            if (!_useGravity)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_ceilingCheckLocation.position, _ceilingCheckRadius);
            Gizmos.DrawWireSphere(_groundCheckLocation.position, _groundCheckRadius);
        }
    }
}