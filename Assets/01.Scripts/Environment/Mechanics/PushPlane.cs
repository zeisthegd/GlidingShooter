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
        public ForceMode Mode = ForceMode.Impulse;
        protected virtual void OnTriggerEnter(Collider col)
        {
            if (TargetMask.Contains(col.gameObject.layer) && col.gameObject.GetComponent<CharacterController>())
            {
                CharacterController controller = col.gameObject.GetComponent<CharacterController>();
                controller.ExternalForce = controller.Velocity * Multiplier;
            }
        }
    }
}

