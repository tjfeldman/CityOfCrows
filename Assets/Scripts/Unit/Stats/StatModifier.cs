namespace UnitStats {

    public enum StatModType
    {
        Flat,
        PercentBuff,
        PercentDebuff,
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly object Source;

        public StatModifier(float value, StatModType type, object source) {
            Value = value;
            Type = type;
            Source = source;
        }

        public StatModifier(float value, StatModType type) : this (value, type, null) { }

    }
}