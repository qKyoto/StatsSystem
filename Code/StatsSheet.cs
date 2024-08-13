using System;
using System.Collections.Generic;
using Meowtopia.Code.Utilities.RuntimeUtility;

namespace Meowtopia.Code.Game.Stats
{
    public interface IStatsSheet
    {
        public Stat GetStat(StatType type);
        public StatAttribute GetAttribute(StatAttributeType type);
    }
    
    public class StatsSheet : IStatsSheet, IDisposable
    {
        private readonly StatsConfig _config;
        private readonly Dictionary<StatType, Stat> _stats;
        private readonly Dictionary<StatAttributeType, StatAttribute> _attributes;

        private List<Action> _dependencyActions;

        public StatsSheet(StatsConfig config)
        {
            _config = config;
            _stats = new SerializableDictionary<StatType, Stat>();
            _attributes = new SerializableDictionary<StatAttributeType, StatAttribute>();
            
            RegisterStats();
            RegisterAttributes();
            RegisterStatDependencies();
        }

        public void Dispose()
        {
            foreach ((StatAttributeType _, StatAttribute statAttribute) in _attributes)
                statAttribute.Dispose();
        }

        public Stat GetStat(StatType type)
        {
            return _stats[type];
        }

        public StatAttribute GetAttribute(StatAttributeType type)
        {
            return _attributes[type];
        }

        private void RegisterStats()
        {
            foreach (StatType statType in _config.DefinedStatTypes)
                _stats.Add(statType, new Stat(statType, _config.GetStatBaseValue(statType), _config.StatDependencyMap));
        }

        private void RegisterAttributes()
        {
            foreach (StatAttributeType attributeType in _config.DefinedStatAttributeTypes)
            {
                Stat traceableStat = GetStat(_config.GetDependencyForAttribute(attributeType));
                _attributes.Add(attributeType, new StatAttribute(traceableStat, attributeType));
            }
        }

        private void RegisterStatDependencies()
        {
            if (_config.StatDependencyMap == null)
                return;

            _config.StatDependencyMap.Initialize(this);
            
            foreach (StatType type in _stats.Keys)
            {
                Stat stat = _stats[type];
                StatType[] dependencies = _config.StatDependencyMap.Dependency[type];

                if (dependencies == null)
                    continue;
                
                foreach (StatType dependencyStat in dependencies)
                    _stats[dependencyStat].StatUpdated += () => { stat.Validate(); };
            }
        }
    }
}