using System;
using UnityEngine;

namespace Code.Game.Stats
{
    [Serializable]
    public class StatModifier
    {
        [SerializeField] private StatModifierType _modifierType;
        [SerializeField] private float _value;

        public StatModifierType ModifierType => _modifierType;
        public int Order => (int)ModifierType;
        public object Source { get; }

        public float Value => _value;

        public StatModifier(StatModifierType modifierType, float value, object source = null)
        {
            _modifierType = modifierType;
            _value = value;
            Source = source;
        }
    }
}
