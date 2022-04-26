using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public class Projectile : PoolableObject
    {
        [HorizontalLine]
        [Header("Velocity")]
        public float Speed;
        public float OverrideGravity;
        [Header("Components")]
        public DamageOnTouch DamageOnTouch;
        public CharacterController Controller;

        protected Health _health;

        protected override void Awake()
        {
            base.Awake();
            _health = GetComponent<Health>();
            if (Controller == null)
                Controller = GetComponent<CharacterController>();
            if (DamageOnTouch == null)
                DamageOnTouch = GetComponent<DamageOnTouch>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            SimulateGravity();
        }

        public virtual void FlyTowards(Vector3 direction)
        {
            Controller.SetVelocity(direction.normalized * Speed);
        }

        public virtual void SimulateGravity()
        {
            Controller.Body.useGravity = false;
            Controller.AddForce(Vector3.down * OverrideGravity * Time.deltaTime);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _health?.SetHealthAtAwake();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Controller.SetVelocity(Vector3.zero);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
