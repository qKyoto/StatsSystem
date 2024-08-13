using System;
using UnityEngine;

namespace Meowtopia.Code.Game.Stats
{
    public class StatAttribute : IDisposable
    {
        private const float MIN_VALUE = 0;

        private readonly Stat _traceableStat;
        
        private float _currentValue;
        private float _maxValue;
        
        public StatAttributeType StatAttributeType { get; }
        public float MinValue => MIN_VALUE;
        public float MaxValue => _maxValue;
        public float NormalizedValue { get; private set; }
        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                _currentValue = Mathf.Clamp(_currentValue, MIN_VALUE, MaxValue);
                
                if (_maxValue != 0)
                    NormalizedValue = _currentValue / _maxValue;

                AttributeUpdated.Invoke();
            }
        }

        public event Action AttributeUpdated = delegate {  };

        public StatAttribute(Stat traceableStat, StatAttributeType statAttributeType)
        {
            StatAttributeType = statAttributeType;
            NormalizedValue = 1;
            
            _traceableStat = traceableStat;
            _maxValue = _traceableStat.Value;
            _currentValue = _maxValue;
            
            _traceableStat.StatUpdated += OnUpdateTraceableStat;
        }

        public void Dispose()
        {
            _traceableStat.StatUpdated -= OnUpdateTraceableStat;
        }

        private void OnUpdateTraceableStat()
        {
            _maxValue = _traceableStat.Value;
            CurrentValue = NormalizedValue * _maxValue;
        }
    }
}