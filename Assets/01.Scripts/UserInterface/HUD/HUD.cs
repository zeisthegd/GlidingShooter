using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using TMPro;

using Penwyn.Game;
using Penwyn.Tools;

namespace Penwyn.UI
{
    public class HUD : SingletonMonoBehaviour<HUD>
    {
        [Header("Player")]
        public ProgressBar PlayerHealth;
        public ProgressBar PlayerEnergy;
        public TMP_Text PlayerMoney;
        public Button WeaponButton;
        public List<WeaponUpgradeButton> WeaponUpgradeButtons;


        protected Character _localPlayer;
        protected virtual void Awake()
        {
            PlayerManager.Instance.PlayerSpawned += OnPlayerSpawned;
        }

        protected virtual void Update()
        {
            UpdateLevelProgress();
        }

        #region Level HUD

        public virtual void UpdateLevelProgress()
        {

        }

        #endregion

        #region PlayerHUD

        public virtual void SetHealthBar()
        {
            if (PlayerHealth != null)
            {
                PlayerHealth.SetMaxValue(_localPlayer.Health.MaxHealth);
                PlayerHealth.SetValue(_localPlayer.Health.CurrentHealth);
            }
        }

        public virtual void SetEnergyBar()
        {
            if (PlayerEnergy != null)
            {
                PlayerEnergy.SetMaxValue(_localPlayer.Health.MaxHealth);
                PlayerEnergy.SetValue(_localPlayer.Health.CurrentHealth);
            }
        }

        protected virtual void UpdateHealth()
        {
            PlayerHealth.SetValue(_localPlayer.Health.CurrentHealth);
            if (_localPlayer.Health.MaxHealth != PlayerHealth.ActualValue.maxValue)
            {
                PlayerHealth.SetMaxValue(_localPlayer.Health.MaxHealth);
                Debug.Log(PlayerHealth.ActualValue.maxValue);
            }
        }

        protected virtual void UpdateEnergy()
        {
            PlayerEnergy.SetValue(_localPlayer.Energy.CurrentEnergy);
            if (_localPlayer.Energy.MaxEnergy != PlayerEnergy.ActualValue.maxValue)
            {
                PlayerEnergy.SetMaxValue(_localPlayer.Energy.MaxEnergy);
                Debug.Log(PlayerEnergy.ActualValue.maxValue);
            }
        }

        protected virtual void UpdateMoney()
        {
            if (PlayerMoney != null)
                PlayerMoney.SetText(_localPlayer.CharacterMoney.CurrentMoney + "");
        }

        #region Weapon Upgrades

        public virtual void LoadAvailableUpgrades()
        {
            if (_localPlayer.CharacterWeaponHandler.CurrentWeapon.CurrentData.Upgrades.Count <= 0)
                return;
            Time.timeScale = 0;
            for (int i = 0; i < _localPlayer.CharacterWeaponHandler.CurrentWeapon.CurrentData.Upgrades.Count; i++)
            {
                WeaponUpgradeButtons[i].Set(_localPlayer.CharacterWeaponHandler.CurrentWeapon.CurrentData.Upgrades[i]);
                WeaponUpgradeButtons[i].gameObject.SetActive(true);
            }
        }

        public virtual void EndWeaponUpgrades()
        {
            SetWeaponButtonIcon();
            Time.timeScale = 1;
            for (int i = 0; i < WeaponUpgradeButtons.Count; i++)
            {
                WeaponUpgradeButtons[i].gameObject.SetActive(false);
            }
        }

        public virtual void ConnectEndWeaponUpgradesEvents()
        {
            for (int i = 0; i < WeaponUpgradeButtons.Count; i++)
            {
                WeaponUpgradeButtons[i].DataChosen += EndWeaponUpgrades;
            }
        }

        public virtual void DisconnectEndWeaponUpgradesEvents()
        {
            for (int i = 0; i < WeaponUpgradeButtons.Count; i++)
            {
                WeaponUpgradeButtons[i].DataChosen -= EndWeaponUpgrades;
            }
        }

        public virtual void SetWeaponButtonIcon()
        {
            if (WeaponButton != null)
                WeaponButton.image.sprite = _localPlayer.CharacterWeaponHandler.CurrentWeapon.CurrentData.Icon;
        }

        #endregion

        protected virtual void OnPlayerSpawned()
        {
            _localPlayer = PlayerManager.Instance.LocalPlayer;
            SetHealthBar();
            SetEnergyBar();
            SetWeaponButtonIcon();
            ConnectEvents();
        }

        #endregion

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }

        public virtual void ConnectEvents()
        {
            _localPlayer.Health.OnChanged += UpdateHealth;
            _localPlayer.Energy.OnChanged += UpdateEnergy;
            // _localPlayer.CharacterMoney.MoneyChanged += UpdateMoney;
            _localPlayer.CharacterWeaponHandler.CurrentWeapon.RequestUpgradeEvent += LoadAvailableUpgrades;
            ConnectEndWeaponUpgradesEvents();
        }

        public virtual void DisconnectEvents()
        {
            _localPlayer.Health.OnChanged -= UpdateHealth;
            _localPlayer.Energy.OnChanged -= UpdateEnergy;
            //_localPlayer.CharacterMoney.MoneyChanged -= UpdateMoney;
            _localPlayer.CharacterWeaponHandler.CurrentWeapon.RequestUpgradeEvent -= LoadAvailableUpgrades;
            PlayerManager.Instance.PlayerSpawned -= OnPlayerSpawned;
            DisconnectEndWeaponUpgradesEvents();
        }
    }
}

