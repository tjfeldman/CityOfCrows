using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace UnitStats {
    [System.Serializable]
    public class HealthSystem
    {
        [SerializeField]
        protected float CurrentHealth;
        [SerializeField]
        protected float MaxHealth;

        public virtual float CurrentHealthValue {
            get {
                return CurrentHealth;
            }
        }

        public virtual float MaxHealthValue {
            get {
                return MaxHealth;
            }
        }

        //Status Effects Lists

        public HealthSystem(float health) {
            CurrentHealth = health;
            MaxHealth = health;
        }

        public void TakeDamage(float damage) {
            CurrentHealth -= (float)Math.Round(damage, 4);
            if (CurrentHealth < 0) {
                CurrentHealth = 0.0f;
            }
        }

        public void Heal(float heal) {
            CurrentHealth += (float)Math.Round(heal, 4);
            if (CurrentHealth > MaxHealth) {
                CurrentHealth = MaxHealth;
            }
        }
    }
}