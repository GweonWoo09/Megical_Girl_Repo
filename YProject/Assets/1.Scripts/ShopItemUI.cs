using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 상점 스크롤뷰의 개별 아이템 UI, 
/// ShopManager가 데이터 주입하면 표시해줌
/// </summary>
public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameAndDescText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    private ShopItemData data;
    private System.Action<ShopItemData> onBuyCallback;

    private void Awake()
    {
        buyButton.onClick.AddListener(OnBuyClicked);
    }

    public void Setup(ShopItemData itemData, System.Action<ShopItemData> callback)
    {
        data = itemData;
        onBuyCallback = callback;

        nameAndDescText.text = $"{itemData.ItemName}: {itemData.ItemDescription}";
        priceText.text = itemData.Price + "G";
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);

    private void OnBuyClicked() => onBuyCallback?.Invoke(data);
}

