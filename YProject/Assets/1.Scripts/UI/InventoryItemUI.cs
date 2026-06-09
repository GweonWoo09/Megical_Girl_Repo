using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 인벤토리 스크롤뷰의 개별 아이템 슬롯 UI입니다.
/// InventoryUI가 데이터를 주입하면 표시만 담당합니다.
/// ShopItemUI와 동일한 구조입니다.
/// </summary>
public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameDescText;
    [SerializeField] private TextMeshProUGUI countText;

    public void Setup(ItemData item, int count)
    {
        if (iconImage != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = item.icon != null;
        }

        if (nameDescText != null) nameDescText.text = $"{item.itemName}: {item.description}";
        if (countText != null) countText.text = $"x{count}";

        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);
}