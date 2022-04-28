using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class Weapon : MonoBehaviour
    {
        [Header("Data")]
        [Expandable]
        public WeaponData CurrentData;
        [HorizontalLine]

        [Header("Graphics")]
        public Animator Animator;
        public SpriteRenderer SpriteRenderer;
        [HorizontalLine]

        [Header("Input")]
        public WeaponInputType InputType;

        [Header("Feedbacks")]
        public Feedbacks UseFeedbacks;

        [Header("Owner")]
        [ReadOnly] public Character Owner;
        [ReadOnly] public Energy Energy;

        [ReadOnly][SerializeField] protected WeaponState _currentWeaponState;
        protected WeaponAim _weaponAim;
        protected WeaponAutoAim _weaponAutoAim;
        protected Coroutine _cooldownCoroutine;

        public event UnityAction RequestUpgradeEvent;

        protected virtual void Awake()
        {
            GetComponents();
        }

        public virtual void Initialization()
        {
        }

        protected virtual void Update()
        {
            UpdateInputEvents();
        }

        public virtual void RequestWeaponUse()
        {
            //*Derive this
            if (_currentWeaponState == WeaponState.WeaponIdle)
            {
                UseWeapon();
            }
        }

        protected virtual void UseWeapon()
        {
            _currentWeaponState = WeaponState.WeaponUse;
            if (UseFeedbacks != null)
                UseFeedbacks.PlayFeedbacks();
            UseEnergy();
        }

        public virtual void UseWeaponTillNoTargetOrEnergy()
        {
            if (Energy.CurrentEnergy <= CurrentData.EnergyCostPerShot)
                return;
            if (_weaponAutoAim)
            {
                _weaponAutoAim.FindTarget();
                if (_weaponAutoAim.Target == null)
                    return;
            }
            RequestWeaponUse();
        }
        public virtual void StartCooldown()
        {
            _currentWeaponState = WeaponState.WeaponCooldown;
            _cooldownCoroutine = StartCoroutine(CooldownCoroutine());
        }

        protected virtual IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(CurrentData.Cooldown);
            _currentWeaponState = WeaponState.WeaponIdle;
        }

        protected virtual void UseEnergy()
        {
            Energy.Use(CurrentData.EnergyCostPerShot);
        }

        protected virtual void OnEnergyChanged()
        {
            if (Energy.CurrentEnergy <= CurrentData.EnergyCostPerShot)
            {
                if (_currentWeaponState == WeaponState.WeaponCooldown && _cooldownCoroutine != null)
                    StopCoroutine(_cooldownCoroutine);
                _currentWeaponState = WeaponState.WeaponNotEnoughEnergy;
            }
            else
            {
                if (_currentWeaponState == WeaponState.WeaponNotEnoughEnergy)
                    _currentWeaponState = WeaponState.WeaponIdle;
            }
            CheckUpgradeRequirements();
        }

        /// <summary>
        /// Load the weapon data from a scriptable data.
        /// </summary>
        public virtual void LoadWeapon(WeaponData data)
        {
            CurrentData = data;
            //SpriteRenderer.sprite = data.Icon;
            SetEnergyRequirements();
        }

        [Button("Load Weapon Data")]
        public virtual void LoadWeapon()
        {
            if (CurrentData != null)
                LoadWeapon(CurrentData);
            else
                Debug.Log("Please insert Weapon Data");
        }

        public virtual void SetEnergyRequirements()
        {
            this.Energy.Set(CurrentData.StartingEnergy, CurrentData.StartingEnergy);
            if (CurrentData.RequiresHealth)
            {
                if (Energy != null)
                {
                    Energy.OnChanged += OnEnergyChanged;
                }
                else
                {
                    Debug.LogWarning($"No health assigned to {Owner.name} although this {CurrentData.Name} requires energy!");
                }
            }
        }

        #region Upgrade

        [Button("Reques Upgrade", EButtonEnableMode.Playmode)]
        public virtual void RequestUpgrade()
        {
            RequestUpgradeEvent?.Invoke();
        }

        [Button("Upgrade (Random Data)")]
        public virtual void RandomUpgrade()
        {
            Upgrade(CurrentData.Upgrades[Randomizer.RandomNumber(0, CurrentData.Upgrades.Count)]);
        }

        public virtual void Upgrade(WeaponData data)
        {
            if (CurrentData != null)
            {
                if (CurrentData.Upgrades != null)
                {
                    LoadWeapon(data);
                    Owner.Health.Set(Owner.Health.CurrentHealth, CurrentData.RequiredUpgradeValue);
                }
                else
                {
                    Debug.Log("Max level reached!");
                }
            }
            else
                Debug.Log("Please insert Weapon Data!");
        }

        public virtual void CheckUpgradeRequirements()
        {
            if (CurrentData.AutoUpgrade)
            {
                if (Owner.Health.CurrentHealth == CurrentData.RequiredUpgradeValue)
                    RequestUpgrade();
            }
        }

        #endregion

        #region  Input Events
        public virtual void UpdateInputEvents()
        {
            if (InputType == WeaponInputType.NormalAttack && InputReader.Instance.IsHoldingNormalAttack)
            {
                UseWeaponTillNoTargetOrEnergy();
            }
        }
        #endregion


        public virtual void GetComponents()
        {
            _weaponAim = GetComponent<WeaponAim>();
            _weaponAutoAim = GetComponent<WeaponAutoAim>();
            Energy = GetComponent<Energy>();
        }

        public virtual void OnEnable()
        {
            _currentWeaponState = WeaponState.WeaponIdle;
        }

        public virtual void OnDisable()
        {
            StopAllCoroutines();
            if (CurrentData.RequiresHealth && Owner.Health != null)
                Owner.Health.OnChanged -= OnEnergyChanged;
        }


    }
}
