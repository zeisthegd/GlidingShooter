using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;
using Penwyn.Tools;

using Photon.Pun;
namespace Penwyn.Game
{
    public class Health : MonoBehaviourPun
    {
        [Header("Health")]
        public float StartingHealth = 10;
        public bool DeactivateOnDeath = false;
        [Header("Invincible")]
        public bool Invincible = false;
        public Color InvincibleFlickerColor = Color.yellow;

        [Header("Invulnerable")]
        public float InvulnerableDuration = 1;
        [Header("Damaged Feedback")]
        public Color DamageTakenFlickerColor = Color.red;

        [Header("Feedbacks")]
        public Feedbacks HitFeedbacks;
        public Feedbacks DeathFeedbacks;

        [SerializeField][ReadOnly] protected float _health = 0;
        [SerializeField][ReadOnly] protected float _maxHealth = 0;
        protected float _invulnerableTime = 0;
        protected bool _currentlyInvulnerable = false;
        protected Character _character;
        protected HealthBar _healthBar;

        public event UnityAction OnChanged;
        public event UnityAction<Character> OnDeath;

        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
            SetHealthAtAwake();
        }

        protected virtual void Start()
        {
            CreateHealthBar();
        }

        public virtual void SetHealthAtAwake()
        {
            _maxHealth = StartingHealth;
            _health = StartingHealth;
        }

        #region Damage Taken

        public virtual void Take(float damage)
        {
            if (_health > 0 && !_currentlyInvulnerable && !Invincible)
            {
                _health -= damage;
                _health = Mathf.Clamp(_health, 0, _maxHealth);
                OnChanged?.Invoke();
                if (HitFeedbacks != null)
                    HitFeedbacks.PlayFeedbacks();
                if (_health > 0)
                {
                    if (photonView != null)
                        photonView.RPC(nameof(RPC_Take), RpcTarget.Others, _health, damage);
                    MakeInvulnerable();
                }
                else
                {
                    if (DeathFeedbacks != null)
                        DeathFeedbacks.PlayFeedbacks();
                    Kill();
                }
            }
        }

        [PunRPC]
        public virtual void RPC_Take(float newHP, float damage)
        {
            _health = newHP;
            MakeInvulnerable();
        }

        [PunRPC]
        public virtual void RPC_Kill()
        {
            _health = 0;
        }

        /// <summary>
        /// Lose flat HP. No invulnerable started.
        /// </summary>
        /// <param name="health">Amount</param>
        public virtual void Lose(float health)
        {
            _health -= health;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            OnChanged?.Invoke();
            if (_health <= 0)
                Kill();
        }

        /// <summary>
        /// Get a flat amount of HP, positive value only.
        /// </summary>
        /// <param name="health"></param>
        public virtual void Get(float health)
        {
            if (health < 0)
                return;
            _health += health;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            OnChanged?.Invoke();
        }

        /// <summary>
        /// Get a flat amount of HP, positive value only.
        /// </summary>
        /// <param name="curretHealth">Current HP</param>
        /// <param name="maxHealth">Max HP</param>
        public virtual void Set(float curretHealth, float maxHealth)
        {
            _maxHealth = maxHealth;
            _health = curretHealth;
            OnChanged?.Invoke();
        }

        #endregion

        #region Kill
        public virtual void Kill()
        {
            _health = 0;
            OnDeath?.Invoke(_character);
            if (DeactivateOnDeath)
                gameObject.SetActive(false);
        }
        #endregion

        #region Invulnerable and Invincible
        public void MakeInvulnerable()
        {
            if (InvulnerableDuration > 0)
                StartCoroutine(PerformInvulnerable());
        }

        public virtual void MakeInvincible(float duration)
        {
            Invincible = true;
            if (duration > 0)
            {
                StartCoroutine(InvincibleCoroutine(duration));
            }
            else
            {
                //TODO change shader or soemthing.
            }
        }

        protected virtual IEnumerator PerformInvulnerable()
        {
            _currentlyInvulnerable = true;
            _invulnerableTime = 0;
            _character.MeshRenderer.material.color = DamageTakenFlickerColor;
            while (_invulnerableTime < InvulnerableDuration)
            {
                _invulnerableTime += Time.deltaTime;
                yield return null;
            }
            ResetColor();
            _currentlyInvulnerable = false;
        }

        protected virtual IEnumerator InvincibleCoroutine(float duration)
        {
            _invulnerableTime = 0;
            _character.MeshRenderer.material.color = InvincibleFlickerColor;
            while (_invulnerableTime < duration)
            {
                _invulnerableTime += Time.deltaTime;
                yield return null;
            }
            ResetColor();
            Invincible = false;
        }
        #endregion

        protected virtual void CreateHealthBar()
        {
            _healthBar = GetComponent<HealthBar>();
            if (_healthBar)
                _healthBar.Initialization();
        }

        public virtual void ResetColor()
        {
            if (_character != null && _character.MeshRenderer != null)
                _character.MeshRenderer.material.color = Color.white;
        }

        public virtual void Reset()
        {
            Set(StartingHealth, StartingHealth);
            StopAllCoroutines();
            ResetColor();
        }

        public virtual void OnEnable()
        {
            _currentlyInvulnerable = false;
            _invulnerableTime = 0;
            if (_healthBar)
                _healthBar.SetHealthSlidersValue();
        }

        public virtual void OnDisable()
        {
            Reset();
        }

        public Character Character { get => _character; }
        public float CurrentHealth { get => _health; }
        public float MaxHealth { get => _maxHealth; }
    }
}
