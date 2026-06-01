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
        // buyButton�� null üũ
        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyClicked);
        else
            Debug.LogError($"[ShopItemUI] buyButton�� ������� �ʾҽ��ϴ�! ������: {gameObject.name}");
    }

    public void Setup(ShopItemData itemData, System.Action<ShopItemData> callback)
    {
        // ��� �ʵ尡 null���� ��Ȯ�ϰ� �α׷� �˷��ݴϴ�
        if (nameAndDescText == null)
        {
            Debug.LogError($"[ShopItemUI] nameAndDescText�� ������� �ʾҽ��ϴ�! ������ �ν����͸� Ȯ���ϼ���. ������Ʈ: {gameObject.name}");
            return;
        }
        if (priceText == null)
        {
            Debug.LogError($"[ShopItemUI] priceText�� ������� �ʾҽ��ϴ�! ������ �ν����͸� Ȯ���ϼ���. ������Ʈ: {gameObject.name}");
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
