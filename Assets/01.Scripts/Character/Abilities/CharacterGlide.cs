using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CharacterGlide : CharacterAbility
    {
        public float MaxSpeed = 30;
        public float MaxDrag = 6;
        public float DragResetFactor = 1;
        public float UpForceResetFactor = 1;
        protected float _dragPercentage = 100;
        protected bool _isGliding = false;
        protected float _maxSpeed = 0;

        public override void AwakeAbility(Character character)
        {
            base.AwakeAbility(character);
        }

        public override void UpdateAbility()
        {
            base.UpdateAbility();
        }

        public override void FixedUpdateAbility()
        {
            base.FixedUpdateAbility();
            Glide();
        }

        public virtual void StartGliding()
        {

            StopCoroutine(ResetDragToZero());
        }
        public virtual void StopGliding()
        {
            _isGliding = false;
            StartCoroutine(ResetDragToZero());
        }
        public virtual void Glide()
        {
            if (InputReader.Instance.IsHoldingGlide)
            {
                Debug.Log(Camera.main.transform.eulerAngles.x);
                float camAngle = Camera.main.transform.eulerAngles.x > 180 ? 360 - Camera.main.transform.eulerAngles.x : Camera.main.transform.eulerAngles.x;
                if (IsLookingUp)
                {
                    StartCoroutine(ResetDragToZero());
                    if (_maxSpeed > 0 && _controller.Velocity.magnitude < MaxSpeed)
                    {
                        _controller.AddForce(Camera.main.transform.forward * _maxSpeed * (1 - _dragPercentage), ForceMode.Impulse);
                        _maxSpeed -= Time.deltaTime * UpForceResetFactor;
                        Debug.DrawRay(_character.Position, Camera.main.transform.forward * _maxSpeed, Color.green);
                    }
                }
                else
                {
                    StopCoroutine(ResetDragToZero());
                    _dragPercentage = 1 - Mathf.Abs(camAngle / 90);
                    _controller.Body.drag = MaxDrag * _dragPercentage;

                    if (_controller.Velocity.magnitude > _maxSpeed)
                        _maxSpeed = _controller.Velocity.magnitude;
                }
            }
        }

        public virtual IEnumerator ResetDragToZero()
        {
            float drag = _controller.Body.drag;
            while (_controller.Body.drag > 0)
            {
                _controller.Body.drag -= (Time.deltaTime * DragResetFactor);
                yield return null;
            }
        }

        public override void ConnectEvents()
        {
            base.ConnectEvents();
            InputReader.Instance.GlidePressed += StartGliding;
            InputReader.Instance.GlideReleased += StopGliding;
        }

        public override void DisconnectEvents()
        {
            base.DisconnectEvents();
            InputReader.Instance.GlidePressed -= StartGliding;
            InputReader.Instance.GlideReleased -= StopGliding;
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public bool IsLookingUp => Camera.main.transform.eulerAngles.x > 180;
    }

}