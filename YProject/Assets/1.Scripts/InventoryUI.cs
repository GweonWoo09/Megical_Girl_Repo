using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class InventoryUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField] private GameObject InventPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("스크롤뷰")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private InventoryItemUI itemPrefab;

    private readonly List<InventoryItemData> itemDataList = new();
    private readonly List<InventoryItemUI> pooledItems = new();

    void Start()
    {
        openButton?.onClick.AddListener(OpenInvent);
        closeButton?.onClick.AddListener(CloseInvent);

        LoadItemData();
        CloseInvent();
    }

    private void LoadItemData()
    {
        itemDataList.Clear();

        // 실제 프로젝트에서는 ScriptableObject나 JSON으로 교체하세요
        itemDataList.Add(new InventoryItemData
        {
            ItemName = "방지권",
            ItemDescription = "강화 실패 시 파괴를 1회 방지합니다.",
        });

        for (int i = 1; i <= 10; i++)
        {
            itemDataList.Add(new InventoryItemData
            {
                ItemName = $"일반 아이템 {i}",
                ItemDescription = "테스트용 아이템입니다.",
            });
        }
    }

    public void OpenInvent()
    {
        RefreshScrollView();
        InventPanel.SetActive(true);
    }

    public void CloseInvent() => InventPanel.SetActive(false);

    private void RefreshScrollView()
    {
        // 풀에서 재사용하거나 새로 생성
        for (int i = 0; i < itemDataList.Count; i++)
        {
            InventoryItemUI ui = (i < pooledItems.Count)
                ? pooledItems[i]
                : CreatePooledItem();
        }

        // 남은 풀 아이템 숨기기
        for (int i = itemDataList.Count; i < pooledItems.Count; i++)
            pooledItems[i].Hide();
    }

    private InventoryItemUI CreatePooledItem()
    {
        InventoryItemUI ui = Instantiate(itemPrefab, contentParent);
        pooledItems.Add(ui);
        return ui;
    }
}
//상점에서 사거나 강화 실패로 얻은 아이템들을 인벤토리에 넣는 시스템을 추가하고 싶습니다.
//파일 이름은 InventoryUI로 정하고 ShopUI에서 