using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 데이터를 관리하는 싱글턴입니다.
/// ShopManager, EnhanceManager에서 아이템을 받아 보관하고
/// 변경이 생길 때마다 이벤트로 InventoryUI에 알립니다.
/// </summary>
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    // 인벤토리 변경 시 InventoryUI가 자동으로 갱신됩니다.
    public static event Action OnInventoryChanged;

    // 아이템 데이터 → 보유 수량 딕셔너리
    private readonly Dictionary<ItemData, int> inventory = new();

    // 외부에서 읽기 전용으로 인벤토리 접근
    public IReadOnlyDictionary<ItemData, int> Inventory => inventory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ── 아이템 추가 ─────────────────────────────────────────────────────────
    /// <summary>인벤토리에 아이템을 추가합니다.</summary>
    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null)
        {
            Debug.LogWarning("[ItemManager] null 아이템은 추가할 수 없습니다.");
            return;
        }

        if (inventory.ContainsKey(item))
            inventory[item] += amount;
        else
            inventory[item] = amount;

        Debug.Log($"[인벤토리] '{item.itemName}' x{amount} 획득 (보유: {inventory[item]})");
        OnInventoryChanged?.Invoke();
    }

    // ── 아이템 제거 ─────────────────────────────────────────────────────────
    /// <summary>인벤토리에서 아이템을 제거합니다. 수량 부족 시 false 반환.</summary>
    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || !inventory.ContainsKey(item) || inventory[item] < amount)
        {
            Debug.LogWarning($"[ItemManager] '{item?.itemName}' 제거 실패: 수량 부족");
            return false;
        }

        inventory[item] -= amount;

        if (inventory[item] <= 0)
            inventory.Remove(item); // 수량 0이면 목록에서 제거

        OnInventoryChanged?.Invoke();
        return true;
    }

    // ── 보유 수량 조회 ──────────────────────────────────────────────────────
    public int GetCount(ItemData item) =>
        inventory.TryGetValue(item, out int count) ? count : 0;
}