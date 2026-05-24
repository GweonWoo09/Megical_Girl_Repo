using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

/// <summary>
/// 상점 UI 관리
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("스크롤뷰")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private ShopItemUI itemPrefab;

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

        // 실제 프로젝트에서는 ScriptableObject나 JSON으로 교체하세요
        itemDataList.Add(new ShopItemData
        {
            ItemName = "방지권",
            ItemDescription = "강화 실패 시 파괴를 1회 방지합니다.",
            Price = 100
        });

        for (int i = 1; i <= 10; i++)
        {
            itemDataList.Add(new ShopItemData
            {
                ItemName = $"일반 아이템 {i}",
                ItemDescription = "테스트용 아이템입니다.",
                Price = i * 10
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
        // 풀에서 재사용하거나 새로 생성
        for (int i = 0; i < itemDataList.Count; i++)
        {
            ShopItemUI ui = (i < pooledItems.Count)
                ? pooledItems[i]
                : CreatePooledItem();

            ui.Setup(itemDataList[i], OnItemPurchased);
        }

        // 남은 풀 아이템 숨기기
        for (int i = itemDataList.Count; i < pooledItems.Count; i++)
            pooledItems[i].Hide();
    }

    private ShopItemUI CreatePooledItem()
    {
        ShopItemUI ui = Instantiate(itemPrefab, contentParent);
        pooledItems.Add(ui);
        return ui;
    }

    private void OnItemPurchased(ShopItemData item)
    {
        // GameDataManager로 재화 차감 (잔액 부족 시 자동으로 false 반환)
        if (GameDataManager.SpendMoney(item.Price))
        {
            Debug.Log($"[상점] '{item.ItemName}' 구매 완료!");
            // TODO: 인벤토리에 아이템 추가
        }
        else
        {
            Debug.Log($"[상점] 재화가 부족합니다. (필요: {item.Price}G)");
            // TODO: 재화 부족 UI 표시
        }
    }
}
