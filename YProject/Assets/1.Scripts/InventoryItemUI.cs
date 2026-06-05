using TMPro;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameAndDescText;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private InventoryItemData data;
    private System.Action<InventoryItemData> onBuyCallback;

    public void Setup(InventoryItemData itemData, System.Action<InventoryItemData> callback)
    {
        // null 판독하기
        if (itemNameAndDescText == null)
        {
            Debug.LogError($"[ShopItemUI] itemNameText가 연결되지 않았습니다! 프리팹 인스펙터를 확인하세요. 오브젝트: {gameObject.name}");
            return;
        }
        if (itemCountText == null)
        {
            Debug.LogError($"[ShopItemUI] itemCountText가 연결되지 않았습니다! 프리팹 인스펙터를 확인하세요. 오브젝트: {gameObject.name}");
            return;
        }

        data = itemData;
        onBuyCallback = callback;

        itemNameAndDescText.text = $"{itemData.ItemName}: {itemData.ItemDescription}";
        //itemCountText.text =  + " 개";
        gameObject.SetActive(true);
    }

    public void Hide() => gameObject.SetActive(false);  

}