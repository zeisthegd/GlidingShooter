using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CharacterHover : CharacterAbility
    {
        public float HoverForce = 1;
        public override void AwakeAbility(Character character)
        {
            base.AwakeAbility(character);
        }

        public override void UpdateAbility()
        {
            base.UpdateAbility();

            if (AbilityAuthorized && InputReader.Instance.IsHoldingJump && _controller.Velocity.y < 0)
            {
                _controller.AddForce(Vector3.up * HoverForce * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                if (_character.States.CurrentState == CharacterAbilityStates.Hovering)
                {
                    _character.States.ChangeState(CharacterAbilityStates.OnAir);
                }
            }
        }

        public override void FixedUpdateAbility()
        {
            base.FixedUpdateAbility();
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
    }

}
