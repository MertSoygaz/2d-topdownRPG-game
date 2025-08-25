using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(menuName = "Create WaveData", fileName = "WaveData", order = 0)]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private List<WavePair> waves;
    
        public List<WavePair> Waves => waves;
    }

    [System.Serializable]
    public struct WavePair
    {
        public int enemyCount;
        public int strongEnemyCount;
    }
}