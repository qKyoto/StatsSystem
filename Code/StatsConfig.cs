using System;
using System.Collections.Generic;
using System.Linq;
using HornyAttributes;
using Meowtopia.Code.Constants.Data;
using UnityEditor;
using UnityEngine;

namespace Meowtopia.Code.Game.Stats
{
    [Serializable]
    public class StatDefinition
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private float _baseValue;

        public StatType StatType => _statType;
        public float BaseValue => _baseValue;

#if UNITY_EDITOR
        public void SetBaseValue(float value) => _baseValue = value;
#endif
    }

    [Serializable]
    public class StatAttributeDefinition
    {
        [SerializeField] private StatAttributeType _attributeType;
        [SerializeField] private StatType _statType;

        public StatAttributeType AttributeType => _attributeType;
        public StatType StatType => _statType;
    }
    
    [CreateAssetMenu(fileName = "Stats Config", menuName = DataPaths.STATS + "Stats Config")]
    public class StatsConfig : ScriptableObject
    {
        [SerializeField] private StatDefinition[] _statDefinitions;
        [SerializeField] private StatAttributeDefinition[] _statAttributeDefinitions;
        [SerializeReference, SubclassSelector] private StatDependencyMap _statDependencyMap; 
        
        public IEnumerable<StatType> DefinedStatTypes => _statDefinitions.Select(definition => definition.StatType);
        public IEnumerable<StatAttributeType> DefinedStatAttributeTypes => _statAttributeDefinitions.Select(definition => definition.AttributeType);
        public StatDependencyMap StatDependencyMap => _statDependencyMap;
        
        public StatType GetDependencyForAttribute(StatAttributeType statAttributeType)
        {
            return _statAttributeDefinitions.First(definition => definition.AttributeType == statAttributeType).StatType;
        }

        public float GetStatBaseValue(StatType statType)
        {
            return _statDefinitions.First(definition => definition.StatType == statType).BaseValue;
        }

#if UNITY_EDITOR
        public void SetStatBaseValue(StatType statType, float value)
        {
            _statDefinitions.First(definition => definition.StatType == statType).SetBaseValue(value);;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
