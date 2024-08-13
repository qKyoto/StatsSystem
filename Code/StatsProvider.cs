using UnityEngine;

namespace Meowtopia.Code.Game.Stats
{
    public class StatsProvider : MonoBehaviour
    {
        [SerializeField] private StatsConfig _statsConfig;
        
        private StatsSheet _statsSheet;
        
        public IStatsSheet StatsSheet => _statsSheet;

#if UNITY_EDITOR
        public StatsConfig StatsConfig => _statsConfig;
#endif

        private void Awake()
        {
            _statsSheet = new StatsSheet(_statsConfig);
        }

        private void OnDestroy()
        {
            _statsSheet.Dispose();
        }
    }
}