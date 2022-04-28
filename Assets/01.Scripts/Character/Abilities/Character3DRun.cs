using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class Character3DRun : CharacterAbility
    {
        [Header("Speed")]
        public float RunSpeed = 5;
        public float MaxSpeed = 5;
        public bool UseRawInput = true;
        public ControlType Type;
        [Header("Feedbacks")]
        public ParticleSystem Dust;

        public override void AwakeAbility(Character character)
        {
            base.AwakeAbility(character);
        }

        public override void UpdateAbility()
        {
            base.UpdateAbility();
            LookAtCamera();
        }

        public override void FixedUpdateAbility()
        {
            if (Type == ControlType.PlayerInput && _controller.IsTouchingGround && AbilityAuthorized)
            {
                if (UseRawInput)
                    RunRaw(InputReader.Instance.MoveInput.normalized);
                else
                    RunAccelerate(InputReader.Instance.MoveInput.normalized);
            }
            DustHandling();
        }

        /// <summary>
        /// Set velocity = input direction * speed.
        /// </summary>
        /// <param name="input">Normalized input</param>
        public virtual void RunRaw(Vector2 input)
        {
            Vector3 direction = transform.right * input.x + transform.forward * input.y;
            _controller.SetVelocity(direction * RunSpeed);
        }

        public virtual void RunAccelerate(Vector2 input)
        {
            if (_controller.Velocity.magnitude < MaxSpeed)
            {
                Vector3 direction = transform.right * input.x + transform.forward * input.y;
                _controller.AddForce(direction * RunSpeed, ForceMode.Acceleration);
            }
        }

        public virtual void LookAtCamera()
        {
            _character.transform.localRotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            if (_controller.IsTouchingGround || _controller.IsTouchingWall)
                _character.Model.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        protected virtual void DustHandling()
        {
            if (Dust != null)
            {
                if (_controller.Velocity.magnitude > 0)
                {
                    if (!Dust.isPlaying)
                        Dust.Play();
                }
                else
                {
                    Dust.Stop();
                }
            }
        }

        public override void ConnectEvents()
        {
            base.ConnectEvents();
        }

        public override void DisconnectEvents()
        {
            base.DisconnectEvents();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public enum ControlType
        {
            PlayerInput,
            Script
        }
    }

}