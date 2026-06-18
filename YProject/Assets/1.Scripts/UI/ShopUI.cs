using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("스크롤뷰")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private ShopItemUI itemPrefab;

    [Header("아이템")]
    // ShopItemData에 ItemData 참조를 추가하여 인벤토리 연동
    [SerializeField] private List<ShopItemData> itemDataList = new();

    private readonly List<ShopItemUI> pooledItems = new();

    private void Start()
    {
        openButton?.onClick.AddListener(OpenShop);
        closeButton?.onClick.AddListener(CloseShop);
        CloseShop();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        RefreshScrollView();
    }

    public void CloseShop() => shopPanel.SetActive(false);

    private void RefreshScrollView()
    {
        for (int i = 0; i < itemDataList.Count; i++)
        {
            ShopItemUI ui = (i < pooledItems.Count) ? pooledItems[i] : CreatePooledItem();
            ui.Setup(itemDataList[i], OnItemPurchased);
        }
        for (int i = itemDataList.Count; i < pooledItems.Count; i++)
            pooledItems[i].Hide();
    }

    private ShopItemUI CreatePooledItem()
    {
        var ui = Instantiate(itemPrefab, contentParent);
        pooledItems.Add(ui);
        return ui;
    }

    private void OnItemPurchased(ShopItemData item)
    {
        if (!GameDataManager.gdmInstance.SpendMoney(item.Price))
        {
            Debug.Log($"[상점] 재화 부족. 필요: {item.Price}G");
            return;
        }

        Debug.Log($"[상점] '{item.ItemName}' 구매 완료!");

        // LinkedItem이 연결되어 있으면 인벤토리에 추가
        if (item.LinkedItem != null)
            ItemManager.imInstance.AddItem(item.LinkedItem);
        else
            Debug.LogWarning($"[상점] '{item.ItemName}'에 LinkedItem이 연결되지 않았습니다.");
    }
}