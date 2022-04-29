using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Penwyn.Game
{
    public class PlayerWeaponMovementController : MonoBehaviour
    {
        public enum State
        {
            Hovering, //Hovering next to Player
            MoveToPosition //Hinting something???
        }

        public Vector3 TargetOffset;
        public AnimationCurve DistanceToSpeedCurve;
        public AnimationCurve MoveToPositionCurve;
        public bool EnableHoverWobbling;

        private Vector3 _moveToPositionEnd;
        private float _moveToPositionDuration;
        private float _moveToPositionTime;
        private float _stateCurrentTime;
        private PlayerWeaponMovementController.State _currentState;

        private Weapon _playerWeapon;

        void Start()
        {
            _playerWeapon = GetComponent<Weapon>();
            _currentState = State.Hovering;
            this.transform.SetParent(null);
        }

        void Update()
        {
            UpdateState();
            TargetFoundHandling();
        }

        protected virtual void UpdateState()
        {
            switch (_currentState)
            {
                case State.Hovering:
                    UpdateHovering();
                    break;
                case State.MoveToPosition:
                    UpdateMoveToPosition();
                    break;
                default:
                    break;
            }
            _stateCurrentTime += Time.deltaTime;
        }

        /// <summary>
        /// Move to the target position.
        /// Wobbling means moving around the target position.
        /// </summary>
        protected virtual void UpdateHovering()
        {
            Vector3 targetPos = _moveToPositionEnd;
            targetPos += TargetOffset;
            if (EnableHoverWobbling)
                targetPos += HoverOffset;

            Vector3 dirToPlayer = targetPos - this.transform.position;
            this.transform.position += dirToPlayer.normalized * DistanceToSpeedCurve.Evaluate(dirToPlayer.magnitude) * Time.deltaTime;
        }

        /// <summary>
        /// Move to a target position in a set duration (sec).
        /// </summary>
        protected virtual void UpdateMoveToPosition()
        {
            _moveToPositionTime += Time.deltaTime;
            Vector3 targetPos = _moveToPositionEnd;
            if (EnableHoverWobbling)
                targetPos += HoverOffset;

            Vector3 dirToTarget = targetPos - this.transform.position;
            if (_moveToPositionDuration > 0)
                this.transform.position += dirToTarget.normalized * MoveToPositionCurve.Evaluate(1 - _moveToPositionTime / _moveToPositionDuration) * Time.deltaTime;
        }

        /// <summary>
        /// If the auto aim find a target, move to the middle position between the player and that target.
        /// Else move back and hover around the player.
        /// </summary>
        protected virtual void TargetFoundHandling()
        {
            if (_playerWeapon?.WeaponAutoAim && _playerWeapon?.WeaponAutoAim.Target != null)
            {
                MoveToTarget(this.PlayerTransform.position + (_playerWeapon.WeaponAutoAim.Target.transform.position - this.PlayerTransform.position) / 2F);
            }
            else
                MoveBackToPlayer();
        }

        /// <summary>
        /// Change the target to player and the state to Hovering.
        /// </summary>
        public void MoveBackToPlayer()
        {
            MoveToTarget(PlayerTransform.position);
        }

        /// <summary>
        /// Change the target and the state to Hovering.
        /// </summary>
        public void MoveToTarget(Vector3 target)
        {
            _moveToPositionEnd = target;
            _currentState = State.Hovering;
        }

        /// <summary>
        /// Set the target position, duration to move to that position.
        /// Change the state to MoveToPosition.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="duration">The duration of the movement.</param>
        public void MoveToPosition(Vector3 position, float duration)
        {
            _moveToPositionTime = 0;
            _moveToPositionDuration = duration;
            _moveToPositionEnd = position;
            _currentState = State.MoveToPosition;
        }

        public Transform PlayerTransform
        {
            get
            {
                return _playerWeapon.Owner.transform;
            }
        }

        /// <summary>
        /// Hover offset for wobbling.
        /// </summary>
        public Vector3 HoverOffset
        {
            get
            {
                return new Vector3(Mathf.Sin((this._stateCurrentTime / 2f + 5f) * 6.28318548f) * 1f, Mathf.Sin(this._stateCurrentTime / 3f) * 0.5f, 0f);
            }
        }
    }
}
