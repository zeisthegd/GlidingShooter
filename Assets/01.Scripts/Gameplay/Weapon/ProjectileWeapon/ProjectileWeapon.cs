using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class ProjectileWeapon : Weapon
    {
        public AimType AimType = AimType.Forward;
        protected ObjectPooler _projectilePooler;
        protected Vector3 _target;

        protected override void UseWeapon()
        {
            base.UseWeapon();
            StartCoroutine(IterationCoroutine());
        }

        protected virtual IEnumerator IterationCoroutine()
        {
            if (_weaponAim)
                _weaponAim.enabled = false;
            for (int i = 0; i < CurrentData.Iteration; i++)
            {
                StartCoroutine(UseWeaponCoroutine());
                yield return new WaitForSeconds(CurrentData.DelayBetweenIterations);
            }
            if (_weaponAim)
                _weaponAim.enabled = true;
            if (_weaponAutoAim != null && _weaponAutoAim.enabled == false)
                _weaponAutoAim.enabled = true;
            StartCooldown();
        }

        protected virtual IEnumerator UseWeaponCoroutine()
        {
            float projectileStep = GetProjectileStep();

            gameObject.RotateZ(CurrentData.Angle / 2F);
            for (int i = 0; i < CurrentData.BulletPerShot; i++)
            {
                _target = GetTarget();
                SpawnProjectile(_target);
                if (CurrentData.BulletPerShot > 1)
                {
                    if (CurrentData.DelayBetweenBullets > 0)
                        yield return new WaitForSeconds(CurrentData.DelayBetweenBullets);
                    gameObject.RotateZ(-projectileStep);
                }
            }
        }

        /// <summary>
        /// Create a projectile, direction is based on the weapon's rotation.
        /// </summary>
        public virtual void SpawnProjectile(Vector3 target)
        {
            Projectile projectile = _projectilePooler.PullOneObject().GetComponent<Projectile>();
            projectile.transform.position = this.transform.position;
            projectile.transform.rotation = this.transform.rotation;
            projectile.gameObject.SetActive(true);
            projectile.FlyTowards((target - this.transform.position));
            projectile.SetOwner(this.Owner);
        }

        /// <summary>
        /// Angle distance of each projectile.
        /// </summary>
        protected virtual float GetProjectileStep()
        {
            if (CurrentData.BulletPerShot != 0)
                return 1F * CurrentData.Angle / CurrentData.BulletPerShot;
            return 0;
        }

        public override void LoadWeapon(WeaponData data)
        {
            base.LoadWeapon(data);
            CurrentData.Projectile.DamageOnTouch.DamageDeal = CurrentData.Damage;

            CreateNewPool();
        }

        public virtual Vector3 GetTarget()
        {
            if (_weaponAutoAim != null)
            {
                return _weaponAutoAim.Target.position;
            }
            if (AimType == AimType.Raycast)
            {
                return RaycastTarget();
            }
            if (AimType == AimType.Forward)
            {
                return (transform.position + transform.forward);
            }
            return Vector3.zero;
        }

        public virtual Vector3 RaycastTarget()
        {
            Vector3 target = Vector3.zero;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F));
            ray.origin = CameraManager.Instance.CurrenPlayerCam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                target = hit.point;
            }
            else
            {
                target = ray.origin + ray.direction;
            }

            return target;
        }

        public virtual void CreateNewPool()
        {
            if (_projectilePooler.NoPoolFound())
            {
                _projectilePooler.ObjectToPool = CurrentData.Projectile.gameObject;
                _projectilePooler.ClearPool();
                _projectilePooler.Init();
            }
        }

        public override void GetComponents()
        {
            base.GetComponents();
            _projectilePooler = GetComponent<ObjectPooler>();
        }
    }
    public enum AimType
    {
        Raycast,
        Forward
    }
}
