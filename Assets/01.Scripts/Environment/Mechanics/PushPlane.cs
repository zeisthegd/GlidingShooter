using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
using NaughtyAttributes;
namespace Penwyn.Game
{
    public class PushPlane : MonoBehaviour
    {
        [InfoBox("Add more velocity to the object's passing through this. ForceMode")]
        public LayerMask TargetMask;
        public float Multiplier = 0.5F;

        public Vector3 FixedForce;
        public PushMode PushMode = PushMode.ObjectVelocity;
        public ForceMode ForceMode = ForceMode.Impulse;

        protected virtual void OnTriggerEnter(Collider col)
        {
            if (TargetMask.Contains(col.gameObject.layer) && col.gameObject.GetComponent<CharacterController>())
            {
                CharacterController controller = col.gameObject.GetComponent<CharacterController>();
                if (PushMode == PushMode.ObjectVelocity)
                {
                    controller.ExternalForce = controller.Velocity * Multiplier;
                }

                if (PushMode == PushMode.Bounce)
                {
                    var direction = Vector3.Reflect(controller.Velocity.normalized, Vector3.up);
                    controller.SetVelocity(direction * controller.Velocity.magnitude * Multiplier);
                }

                if (PushMode == PushMode.FixedForce)
                {
                    controller.AddForce(FixedForce * Multiplier, ForceMode);
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision col)
        {
            if (TargetMask.Contains(col.gameObject.layer) && col.gameObject.GetComponent<CharacterController>())
            {
                CharacterController controller = col.gameObject.GetComponent<CharacterController>();
                if (PushMode == PushMode.ObjectVelocity)
                {
                    controller.ExternalForce = controller.Velocity * Multiplier;
                }

                if (PushMode == PushMode.Bounce)
                {
                    var direction = Vector3.Reflect(controller.Velocity.normalized, col.contacts[0].normal);
                    controller.SetVelocity(direction * controller.Velocity.magnitude * Multiplier);
                }

                if (PushMode == PushMode.FixedForce)
                {
                    controller.AddForce(FixedForce * Multiplier, ForceMode);
                }
            }
        }
    }

    public enum PushMode
    {
        ObjectVelocity,
        FixedForce,
        Bounce
    }
}

