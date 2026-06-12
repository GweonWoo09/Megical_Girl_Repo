using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("재화 / 가격")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("파괴 방지권")]
    [SerializeField] private TextMeshProUGUI protectionCountText;
    [SerializeField] private ItemData protectionScrollData;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        GameDataManager.Instance.OnMoneyChanged += RefreshMoneyUI;
        GameDataManager.Instance.OnPriceChanged += RefreshPriceUI;
        ItemManager.OnInventoryChanged += RefreshProtectionCountUI;
    }

    private void OnDisable()
    {
        GameDataManager.Instance.OnMoneyChanged -= RefreshMoneyUI;
        GameDataManager.Instance.OnPriceChanged -= RefreshPriceUI;
        ItemManager.OnInventoryChanged -= RefreshProtectionCountUI;
    }

    private void Start()
    {
        RefreshMoneyUI(GameDataManager.Instance.Money);
        RefreshPriceUI(GameDataManager.Instance.Price);
        RefreshProtectionCountUI();
    }

    private void RefreshMoneyUI(int money)
    {
        if (moneyText != null)
            moneyText.text = money.ToString("N0");
    }

    private void RefreshPriceUI(int price)
    {
        if (priceText != null)
            priceText.text = "가격: " + price.ToString("N0");
    }

    private void RefreshProtectionCountUI()
    {
        if (protectionCountText == null) return;

        int count = (protectionScrollData != null && ItemManager.Instance != null)
            ? ItemManager.Instance.GetCount(protectionScrollData)
            : 0;

        // 보유 수량에 따라 텍스트와 색상을 다르게 표시합니다.
        protectionCountText.text = $"방지권: {count}개";
    }
}