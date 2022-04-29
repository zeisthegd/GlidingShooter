using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;

namespace Penwyn.Game
{
    public class Energy : MonoBehaviour
    {
        public float MaxEnergy = 10;

        public event UnityAction OnChanged;

        [SerializeField][ReadOnly] protected float _energy = 0;
        protected Character _character;

        void Awake()
        {
            _character = GetComponent<Character>();
            _energy = MaxEnergy;
        }

        public virtual void Use(float value)
        {
            if (_energy - value >= 0)
            {
                _energy -= value;
                OnChanged?.Invoke();
            }
            else
            {
                Debug.Log("Not enough energy!");
            }
        }

        public virtual void Add(float addValue)
        {
            Set(_energy + addValue, MaxEnergy);
        }

        public virtual void Set(float newValue, float maxEnergy)
        {
            MaxEnergy = maxEnergy;
            _energy = Mathf.Clamp(newValue, 0, MaxEnergy);
            OnChanged?.Invoke();
        }

        public override string ToString()
        {
            return $"Energy: {CurrentEnergy}/{MaxEnergy}";
        }

        public float CurrentEnergy { get => _energy; }
    }
}

