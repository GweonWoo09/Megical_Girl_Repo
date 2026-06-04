using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameAndDescText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button buyButton;

    private ShopItemData data;
    private System.Action<ShopItemData> onBuyCallback;

    private void Awake()
    {
        // null 판독하기
        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyClicked);
        else
            Debug.LogError($"[ShopItemUI] buyButton이 연결되지 않았습니다! 프리팹: {gameObject.name}");
    }

    public void Setup(ShopItemData itemData, System.Action<ShopItemData> callback)
    {
        // null 판독하기
        if (nameAndDescText == null)
        {
            Debug.LogError($"[ShopItemUI] nameAndDescText가 연결되지 않았습니다! 프리팹 인스펙터를 확인하세요. 오브젝트: {gameObject.name}");
            return;
        }
        if (priceText == null)
        {
            Debug.LogError($"[ShopItemUI] priceText가 연결되지 않았습니다! 프리팹 인스펙터를 확인하세요. 오브젝트: {gameObject.name}");
            return;
        }

        data = itemData;
        onBuyCallback = callback;

        nameAndDescText.text = $"{itemData.ItemName}: {itemData.ItemDescription}";
        priceText.text = itemData.Price + "G";
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);

    private void OnBuyClicked() => onBuyCallback?.Invoke(data);
}
