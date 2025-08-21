using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject effect;
    [SerializeField] protected EnemyData enemyData;

    public GameObject Effect => effect;
    
    public virtual void KillEnemy()
    {
        CoinManager.Instance.AddCoins(enemyData.RewardCoin);
    }
}
