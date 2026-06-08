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

    // ShopItemData에 ItemData 참조를 추가하여 인벤토리 연동
    private readonly List<ShopItemData> itemDataList = new();
    private readonly List<ShopItemUI> pooledItems = new();

    private void Start()
    {
        openButton?.onClick.AddListener(OpenShop);
        closeButton?.onClick.AddListener(CloseShop);
        LoadItemData();
        CloseShop();
    }

    private void LoadItemData()
    {
        itemDataList.Clear();

        // ShopItemData.LinkedItem에 ItemData ScriptableObject를 연결하면
        // 구매 시 자동으로 인벤토리에 추가됨
        itemDataList.Add(new ShopItemData
        {
            ItemName = "방지권",
            ItemDescription = "강화 실패 시 파괴를 1회 방지합니다.",
            Price = 100,
            LinkedItem = null // 인스펙터 연동 or 코드에서 직접 할당
        });

        for (int i = 1; i <= 10; i++)
        {
            itemDataList.Add(new ShopItemData
            {
                ItemName = $"일반 아이템 {i}",
                ItemDescription = "테스트용 아이템입니다.",
                Price = i * 10,
                LinkedItem = null
            });
        }
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
        if (!GameDataManager.SpendMoney(item.Price))
        {
            Debug.Log($"[상점] 재화 부족. 필요: {item.Price}G");
            return;
        }

        Debug.Log($"[상점] '{item.ItemName}' 구매 완료!");

        // LinkedItem이 연결되어 있으면 인벤토리에 추가
        if (item.LinkedItem != null)
            ItemManager.Instance.AddItem(item.LinkedItem);
        else
            Debug.LogWarning($"[상점] '{item.ItemName}'에 LinkedItem이 연결되지 않았습니다.");
    }
}