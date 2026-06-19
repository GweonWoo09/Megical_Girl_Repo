using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager uimInstance { get; private set; }

    [Header("재화 / 가격")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("파괴 방지권")]
    [SerializeField] private TextMeshProUGUI protectionCountText;
    [SerializeField] private ItemData protectionScrollData;

    [Header("재화 획득권")]
    [SerializeField] private TextMeshProUGUI earnMoneyText;
    [SerializeField] private ItemData earnMoneyData;

    private void Awake()
    {
        if (uimInstance != null && uimInstance != this) { Destroy(gameObject); return; }
        uimInstance = this;
    }

    private void OnEnable()
    {
        GameDataManager.gdmInstance.OnMoneyChanged += RefreshMoneyUI;
        GameDataManager.gdmInstance.OnPriceChanged += RefreshPriceUI;
        ItemManager.imInstance.OnInventoryChanged += RefreshProtectionCountUI;
        ItemManager.imInstance.OnEarnMoneyChanged += RefreshEarnMoneyUI;
    }

    private void OnDisable()
    {
        GameDataManager.gdmInstance.OnMoneyChanged -= RefreshMoneyUI;
        GameDataManager.gdmInstance.OnPriceChanged -= RefreshPriceUI;
        ItemManager.imInstance.OnInventoryChanged -= RefreshProtectionCountUI;
        ItemManager.imInstance.OnEarnMoneyChanged -= RefreshEarnMoneyUI;
    }

    private void Start()
    {
        RefreshMoneyUI(GameDataManager.gdmInstance.Money);
        RefreshPriceUI(GameDataManager.gdmInstance.Price);
        RefreshProtectionCountUI();
        RefreshEarnMoneyUI();
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

        int count = (protectionScrollData != null && ItemManager.imInstance != null)
            ? ItemManager.imInstance.GetCount(protectionScrollData)
            : 0;

        protectionCountText.text = $"방지권: {count}개";
    }

    private void RefreshEarnMoneyUI()
    {
        if (earnMoneyText == null) return;

        int count = (earnMoneyData != null && ItemManager.imInstance != null)
            ? ItemManager.imInstance.GetCount(earnMoneyData)
            : 0;

        earnMoneyText.text = $"{count}0/s";
    }
}