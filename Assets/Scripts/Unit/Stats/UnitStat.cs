using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace UnitStats {
    [System.Serializable]
    public class UnitStat
    {
        [SerializeField]
        protected float BaseValue;

        public virtual float Value { 
            get { 
                if (isDirty || BaseValue != _lastBaseValue) { 
                    _lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            } 
        }
        protected bool isDirty = true;
        protected float _value;
        protected float _lastBaseValue = float.MinValue;

        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        public UnitStat() 
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public UnitStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod)) 
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i >= 0; i--) 
            {
                if (statModifiers[i].Source == source) 
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentBuff = 0;
            float sumPercentDebuff = 0;

            foreach (StatModifier mod in statModifiers) 
            {
                if (mod.Type == StatModType.Flat) {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentBuff) {
                    sumPercentBuff += mod.Value;
                }
                else if (mod.Type == StatModType.PercentDebuff) {
                    sumPercentDebuff += mod.Value;
                }
            }

            finalValue *= 1 + sumPercentBuff;
            finalValue *= 1 + sumPercentDebuff;

            return (float)Math.Round(finalValue, 4);
        }
    }
}