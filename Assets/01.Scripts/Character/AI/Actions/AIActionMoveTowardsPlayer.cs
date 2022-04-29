using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class AIActionMoveTowardsPlayer : AIAction
    {
        [Header("Base")]
        public float MinDistance = 2;
        public AnimationCurve DistanceToSpeedCurve;

        protected GameObject _target;
        protected float _randomAngle;

        public override void AwakeComponent(Character character)
        {
            base.AwakeComponent(character);
        }

        public override void UpdateComponent()
        {
            _target = PlayerManager.Instance.LocalPlayer.gameObject;
            if (_target == null)
                return;
            float distanceToPlayer = Vector2.Distance(_target.transform.position, _character.Position);
            if (_target != null && distanceToPlayer > MinDistance)
            {
                Vector3 dirToPlayer = (_target.transform.position - _character.Position).normalized;
                _character.CharacterRun.RunDirection(dirToPlayer);
                _character.transform.LookAt(_target.transform.position);
            }
        }

        public override void StateEnter()
        {
            base.StateEnter();
        }

        public override void StateExit()
        {
            base.StateExit();
        }
    }
}
