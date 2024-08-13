using System.Collections.Generic;

namespace Meowtopia.Code.Game.Stats
{
    public abstract class StatDependencyMap
    {
        protected static StatsSheet StatsSheet { get; private set; }
        protected static readonly Formula IDENTITY = baseValue => baseValue;
        
        public delegate float Formula(float baseValue);

        public virtual Dictionary<StatType, StatType[]> Dependency { get; } = new();
        public virtual Dictionary<StatType, Formula> FormulaMap { get; } = new();

        public void Initialize(StatsSheet statsSheet)
        {
            StatsSheet = statsSheet;
        }
    }

    public class PlayerStatDependencyMap : StatDependencyMap
    {
        public override Dictionary<StatType, StatType[]> Dependency { get; } = new() { };

        public override Dictionary<StatType, Formula> FormulaMap { get; } = new() { };
    }
}