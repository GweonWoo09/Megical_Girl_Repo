using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button useButton;
    [SerializeField] private TextMeshProUGUI useButtonText;

    private ItemData currentItem;

    private void Awake()
    {
        useButton?.onClick.AddListener(OnUseClicked);
    }

    public void Setup(ItemData item, int count)
    {
        currentItem = item;

        if (iconImage != null) { iconImage.sprite = item.icon; iconImage.enabled = item.icon != null; }
        if (nameText != null) nameText.text = item.itemName;
        if (descText != null) descText.text = item.description;
        if (countText != null) countText.text = $"x{count}";

        // 피동 아이템(파괴 방지권, 재화 획득권)은 버튼을 "자동"으로 표시
        if (useButton != null)
        {
            bool isPassive = item.effectType == ItemEffectType.ProtectionScroll
                           || item.effectType == ItemEffectType.EarnMoney;
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
            ItemManager.imInstance.UseItem(currentItem);
    }
}