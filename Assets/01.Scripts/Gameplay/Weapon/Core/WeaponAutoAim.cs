using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public class WeaponAutoAim : MonoBehaviour
    {
        [Header("Mask")]
        public LayerMask TargetsMask;
        public LayerMask ObstacleMask;
        public float CastDistance = 10;

        [ReadOnly] public Transform Target;
        protected Weapon _weapon;
        protected WeaponAim _weaponAim;
        protected List<Transform> _targetList;
        protected List<Collider> _detectionColliders;

        protected virtual void Awake()
        {
            _weapon = GetComponent<Weapon>();
            _weaponAim = GetComponent<WeaponAim>();
        }

        protected virtual void Update()
        {
        }

        public virtual void FindTarget()
        {
            ScanForTargets();
            Aim();
        }

        public virtual void ScanForTargets()
        {
            Target = null;
            _targetList = new List<Transform>();
            _detectionColliders = Physics.OverlapSphere(_weapon.Owner.transform.position, CastDistance, TargetsMask).ToList();

            foreach (Collider collider in _detectionColliders)
            {
                if (!Physics.Raycast(_weapon.Owner.transform.position, collider.transform.position - _weapon.Owner.transform.position, Vector3.Distance(collider.transform.position, _weapon.Owner.transform.position), ObstacleMask) && !_targetList.Contains(collider.transform))
                {
                    _targetList.Add(collider.transform);
                }
            }
            if (_targetList.Count > 0)
            {
                // Sort by distance
                _targetList = _targetList.OrderBy(x => Vector3.Distance(x.transform.position, _weapon.Owner.transform.position)).ToList();
                Target = _targetList[0];
            }
        }

        protected virtual void Aim()
        {
            if (Target != null && Target.gameObject.activeInHierarchy)
            {
                Vector3 dirToTarget = (Target.position - _weapon.transform.position).normalized;
                _weapon.transform.right = dirToTarget;
                Debug.DrawRay(_weapon.transform.position, dirToTarget);
            }
        }
    }
}

