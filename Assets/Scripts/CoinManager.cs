using UnityEngine;
using TMPro;
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;     // Singleton

    private int _coinCount;
    public int  CoinCount {get => _coinCount; set => _coinCount = value; }
    
    [SerializeField] private TMP_Text coinText; 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        _coinCount += amount;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText is not null)
        {
            coinText.text = _coinCount.ToString();
        }
    }
}
