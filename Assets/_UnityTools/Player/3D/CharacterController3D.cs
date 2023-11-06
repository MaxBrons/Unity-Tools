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
using UnityEngine;

namespace UnityTools.Player
{
    /// <summary>
    /// A physics-based 3D character controller class that handles 
    /// player movement, jumping and crouching.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController3D : MonoBehaviour
    {
        // Used for general 3D movement
        [Header("3D General/Top-Down")]
        [Tooltip("The max speed at wich the player travels.")]
        [SerializeField] private float _movementSpeed = 10.0f;
        [Tooltip("The smoothing that is aplied when moving.")]
        [SerializeField] private float _movementSmoothing = 0.05f;
        [Tooltip("The amount that the movement is multiplied by when crouching.")]
        [Range(0, 1)][SerializeField] private float _crouchSpeed = 0.36f;
        [Tooltip("The amount that the movement is multiplied by when sprinting.")]
        [Min(1)][SerializeField] private float _sprintSpeed = 1.5f;

        // Used for a 3D platformer style movement where you use gravity.             
        [Header("3D Platformer")]
        [Tooltip("Used to pick between platformer or top-down mode.")]
        [SerializeField] private bool _useGravity;
        [Tooltip("The force that will be aplied to the player when jumping.")]
        [SerializeField] private float _jumpForce = 600.0f;
        [Tooltip("The layer that is used to check if the player is grounded.")]
        [SerializeField] private LayerMask _obstacleLayer;
        [Tooltip("The location that is used to check if the player is under an obstacle")]
        [SerializeField] private Transform _ceilingCheckLocation;
        [Tooltip("The location that is used to check if the player is grounded.")]
        [SerializeField] private Transform _groundCheckLocation;
        [Tooltip("The radius that is used to check if the player is under an obstacle.")]
        [SerializeField] private float _ceilingCheckRadius = 0.2f;
        [Tooltip("The radius that is used to check if the player is grounded.")]
        [SerializeField] private float _groundCheckRadius = 0.2f;

        public Vector3 Velocity => _rb.velocity;
        public bool UseGravity => _rb.useGravity;

        private Vector2 _velocity = Vector3.zero;
        private Rigidbody _rb;
        private bool _crouching = false;
        private bool _sprinting = false;
        private bool _isGrounded = false;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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
            _isGrounded = Physics.CheckSphere(_groundCheckLocation.position, _groundCheckRadius, _obstacleLayer, QueryTriggerInteraction.Ignore);

            // Add a constant downwards force (gravity) to the player,
            // because Unity's gravity multiplier is not realistic.
            if (!_isGrounded)
                _rb.AddForce(9.81f * Time.fixedDeltaTime * -transform.up, ForceMode.VelocityChange);
        }

        /// <summary>
        /// Move the player in the given direction based on the set 
        /// speed and movement smoothing.
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector2 direction)
        {
            if (_useGravity)
                direction *= _crouching ? _crouchSpeed :
                             _sprinting ? _sprintSpeed :
                                          1.0f;

            Vector2 moveDelta = _movementSpeed * 10.0f * Time.fixedDeltaTime * direction;
            Vector2 result = Vector2.SmoothDamp(new Vector2(_rb.velocity.x, _rb.velocity.z), moveDelta, ref _velocity, _movementSmoothing);

            _rb.velocity = new Vector3(result.x, _rb.velocity.y, result.y);
        }

        /// <summary>
        /// Let the player crouch or stand up, but only
        /// if there are no obstacles above the player's head.
        /// </summary>
        /// <param name="value"></param>
        public void Crouch(bool value)
        {
            if (!_useGravity)
                return;

            _crouching = value;
            if (_crouching)
                return;

            if (Physics.CheckSphere(_ceilingCheckLocation.position, _ceilingCheckRadius, _obstacleLayer, QueryTriggerInteraction.Ignore)) {
                _crouching = true;
            }
        }

        /// <summary>
        /// Add a jump force to the rigidbody when the player is grounded.
        /// </summary>
        public void Jump(bool value)
        {
            MoveUp(value);

            if (!value || !_isGrounded)
                return;

            // Add the impulse force to the player when the gravity is enabled
            _isGrounded = false;
            SetVelocity(new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z));
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        /// <summary>
        /// Set the current sprinting state of the player.
        /// </summary>
        /// <param name="value"></param>
        public void Sprint(bool value)
        {
            _sprinting = value;
            MoveDown(value);
        }


        /// <summary>
        /// Manually set the rigidbody's velocity.
        /// </summary>
        /// <param name="value"></param>
        public void SetVelocity(Vector3 value)
        {
            _rb.velocity = value;
            _velocity = value;
        }

        /// <summary>
        /// Set if you want the player to experience gravity during gameplay.
        /// </summary>
        /// <param name="value"></param>
        private void SetUseGravity(bool value)
        {
            _useGravity = value;
            _rb.useGravity = value;
        }

        /// <summary>
        /// Add a normal upwards force to the player when the gravity is disabled,
        /// instead of the impulse force when the gravity is enabled.
        /// </summary>
        /// <param name="moveDown"></param>
        private void MoveUp(bool moveDown)
        {
            if (_useGravity)
                return;

            SetVelocity(new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z));

            if (moveDown) {
                float yVelocity = (transform.up * _jumpForce / 100.0f).y;
                _rb.AddForce(_rb.velocity - new Vector3(_rb.velocity.x, -yVelocity, _rb.velocity.z), ForceMode.VelocityChange);
            }
        }

        /// <summary>
        /// Add a normal downwards force to the player when the gravity is disabled.
        /// </summary>
        /// <param name="moveDown"></param>
        private void MoveDown(bool moveUp)
        {
            if (_useGravity)
                return;

            SetVelocity(new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z));

            if (moveUp) {
                float yVelocity = (transform.up * _jumpForce / 100.0f).y;
                _rb.AddForce(_rb.velocity - new Vector3(_rb.velocity.x, yVelocity, _rb.velocity.z), ForceMode.VelocityChange);
            }
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