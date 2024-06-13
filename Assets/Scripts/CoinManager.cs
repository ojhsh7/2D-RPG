using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int Coin { get; private set; }
    public Text coinText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoin(int amount)
    {
        Coin += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = ": " + Coin.ToString();
        }
    }
}
