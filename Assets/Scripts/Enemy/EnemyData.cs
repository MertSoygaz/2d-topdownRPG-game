using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData",menuName ="Project/Enemy/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int rewardCoin;

    public int RewardCoin => rewardCoin;
}
