using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public static event Action OnInventoryChanged;

    private readonly Dictionary<ItemData, int> inventory = new();
    public IReadOnlyDictionary<ItemData, int> Inventory => inventory;

    // EnhanceManager 참조 (아이템 효과 적용용)
    [SerializeField] private EnhanceManager enhanceManager;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── 추가 / 제거 ─────────────────────────────────────────────────────────
    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null) return;
        inventory[item] = inventory.TryGetValue(item, out int cur) ? cur + amount : amount;
        Debug.Log($"[인벤토리] '{item.itemName}' x{amount} 획득 (보유: {inventory[item]})");
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || !inventory.TryGetValue(item, out int cur) || cur < amount)
        {
            Debug.LogWarning($"[인벤토리] '{item?.itemName}' 제거 실패: 수량 부족");
            return false;
        }
        inventory[item] = cur - amount;
        if (inventory[item] <= 0) inventory.Remove(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public int GetCount(ItemData item) =>
        inventory.TryGetValue(item, out int c) ? c : 0;

    // ── 아이템 사용 (능동 효과만) ────────────────────────────────────────────
    /// <summary>
    /// InventoryItemUI의 '사용' 버튼에서 호출됩니다.
    /// 파괴 방지권은 피동 효과이므로 사용 버튼에서 호출하지 않습니다.
    /// </summary>
    public void UseItem(ItemData item)
    {
        if (item == null || !inventory.ContainsKey(item))
        {
            Debug.LogWarning("[인벤토리] 사용할 아이템이 없습니다.");
            return;
        }
        if (enhanceManager == null)
        {
            Debug.LogError("[ItemManager] EnhanceManager가 연결되지 않았습니다!");
            return;
        }

        switch (item.effectType)
        {
            case ItemEffectType.ProtectionScroll:
                Debug.Log("[아이템] 파괴 방지권은 강화 실패 시 자동으로 소모됩니다.");
                return; // 수동 사용 불가

            case ItemEffectType.ProbabilityBoost:
                enhanceManager.ApplyProbabilityBoost(item.effectValue);
                break;

            case ItemEffectType.InstantGrowth:
                enhanceManager.ApplyInstantGrowth(item.effectValue);
                break;

            case ItemEffectType.Roulette:
                enhanceManager.ApplyRoulette();
                break;

            default:
                Debug.LogWarning($"[아이템] '{item.itemName}'에 정의된 효과가 없습니다.");
                return;
        }

        // 효과 적용 성공 시 아이템 1개 소모
        RemoveItem(item, 1);
        Debug.Log($"[아이템] '{item.itemName}' 사용 완료.");
    }
}