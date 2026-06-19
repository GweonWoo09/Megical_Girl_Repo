using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 패널 UI를 관리합니다.
/// ItemManager의 OnInventoryChanged 이벤트를 구독하여
/// 아이템이 추가/제거될 때 자동으로 목록을 갱신합니다.
/// ShopManager와 동일한 풀링 구조입니다.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("스크롤뷰")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private InventoryItemUI itemPrefab;

    // 오브젝트 풀
    private readonly List<InventoryItemUI> pooledItems = new();

    private void Awake()
    {
        openButton?.onClick.AddListener(OpenInventory);
        closeButton?.onClick.AddListener(CloseInventory);
    }

    private void OnEnable()
    {
        // 인벤토리가 바뀔 때마다 자동 갱신
        ItemManager.imInstance.OnInventoryChanged += RefreshScrollView;
        ItemManager.imInstance.OnEarnMoneyChanged += RefreshScrollView;
    }

    private void OnDisable()
    {
        ItemManager.imInstance.OnInventoryChanged -= RefreshScrollView;
        ItemManager.imInstance.OnEarnMoneyChanged += RefreshScrollView;
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        RefreshScrollView();
    }

    public void CloseInventory() => inventoryPanel.SetActive(false);

    // ── 스크롤뷰 갱신 ───────────────────────────────────────────────────────
    private void RefreshScrollView()
    {
        var inventory = ItemManager.imInstance.Inventory;
        int index = 0;

        foreach (var (item, count) in inventory)
        {
            InventoryItemUI ui = (index < pooledItems.Count)
                ? pooledItems[index]
                : CreatePooledItem();

            ui.Setup(item, count);
            index++;
        }

        // 남는 슬롯 숨기기
        for (int i = index; i < pooledItems.Count; i++)
            pooledItems[i].Hide();
    }

    private InventoryItemUI CreatePooledItem()
    {
        InventoryItemUI ui = Instantiate(itemPrefab, contentParent);
        pooledItems.Add(ui);
        return ui;
    }
}