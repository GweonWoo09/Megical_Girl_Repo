using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameDescText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button useButton;     // 사용 버튼
    [SerializeField] private TextMeshProUGUI useButtonText; // 버튼 라벨

    private ItemData currentItem;

    private void Awake()
    {
        useButton?.onClick.AddListener(OnUseClicked);
    }

    public void Setup(ItemData item, int count)
    {
        currentItem = item;

        if (iconImage != null) { iconImage.sprite = item.icon; iconImage.enabled = item.icon != null; }
        if (nameDescText != null) nameDescText.text = $"{item.itemName}: {item.description}";
        if (countText != null) countText.text = $"x{count}";

        // 파괴 방지권은 자동 소모이므로 버튼을 "자동"으로 표시하고 비활성화
        if (useButton != null)
        {
            bool isPassive = item.effectType == ItemEffectType.ProtectionScroll;
            useButton.interactable = !isPassive;
            if (useButtonText != null)
                useButtonText.text = isPassive ? "자동" : "사용";
        }

        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);

    private void OnUseClicked()
    {
        if (currentItem != null)
            ItemManager.Instance.UseItem(currentItem);
    }
}