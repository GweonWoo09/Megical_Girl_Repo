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

    private void Awake()
    {
        if (uimInstance != null && uimInstance != this) { Destroy(gameObject); return; }
        uimInstance = this;
    }

    private void OnEnable()
    {
        GameDataManager.gdmInstance.OnMoneyChanged += RefreshMoneyUI;
        GameDataManager.gdmInstance.OnPriceChanged += RefreshPriceUI;
        ItemManager.OnInventoryChanged += RefreshProtectionCountUI;
    }

    private void OnDisable()
    {
        GameDataManager.gdmInstance.OnMoneyChanged -= RefreshMoneyUI;
        GameDataManager.gdmInstance.OnPriceChanged -= RefreshPriceUI;
        ItemManager.OnInventoryChanged -= RefreshProtectionCountUI;
    }

    private void Start()
    {
        RefreshMoneyUI(GameDataManager.gdmInstance.Money);
        RefreshPriceUI(GameDataManager.gdmInstance.Price);
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

        int count = (protectionScrollData != null && ItemManager.imInstance != null)
            ? ItemManager.imInstance.GetCount(protectionScrollData)
            : 0;

        // 보유 수량에 따라 텍스트와 색상을 다르게 표시합니다.
        protectionCountText.text = $"방지권: {count}개";
    }
}