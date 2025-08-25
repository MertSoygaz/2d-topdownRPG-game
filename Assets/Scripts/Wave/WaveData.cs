using System.Collections.Generic;
using UnityEngine;

namespace Wave
{
    [CreateAssetMenu(fileName ="WaveData",menuName ="Project/Wave/Create WaveData")]
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