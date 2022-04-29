using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CharacterControlEnergy : CharacterAbility
    {
        public override void AwakeAbility(Character character)
        {
            base.AwakeAbility(character);
        }

        public virtual void ResetEnergy()
        {
            _character.Energy.Set(_character.Energy.MaxEnergy, _character.Energy.MaxEnergy);
        }

        public override void ConnectEvents()
        {
            base.ConnectEvents();
            _controller.GroundTouched += ResetEnergy;
        }

        public override void DisconnectEvents()
        {
            base.DisconnectEvents();
            _controller.GroundTouched -= ResetEnergy;
        }

    }

}
