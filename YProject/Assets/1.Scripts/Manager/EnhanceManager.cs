using UnityEngine;

public class EnhanceManager : MonoBehaviour
{
    // ── 강화 테이블 ─────────────────────────────────────────────────────────
    private static readonly EnhanceLevelData[] enhanceTable =
    {
        new() { FromLevel =  1, SuccessRate = 100f, EnhanceCost =    50, SellPrice =       0 },
        new() { FromLevel =  2, SuccessRate =  90f, EnhanceCost =    80, SellPrice =      50 },
        new() { FromLevel =  3, SuccessRate =  85f, EnhanceCost =   120, SellPrice =     120 },
        new() { FromLevel =  4, SuccessRate =  80f, EnhanceCost =   170, SellPrice =     250 },
        new() { FromLevel =  5, SuccessRate =  75f, EnhanceCost =   230, SellPrice =     500 },
        new() { FromLevel =  6, SuccessRate =  70f, EnhanceCost =   320, SellPrice =     900 },
        new() { FromLevel =  7, SuccessRate =  65f, EnhanceCost =   450, SellPrice =    1500 },
        new() { FromLevel =  8, SuccessRate =  60f, EnhanceCost =   620, SellPrice =    2500 },
        new() { FromLevel =  9, SuccessRate =  55f, EnhanceCost =   850, SellPrice =    4000 },
        new() { FromLevel = 10, SuccessRate =  50f, EnhanceCost =  1100, SellPrice =    7000 },
        new() { FromLevel = 11, SuccessRate =  45f, EnhanceCost =  1450, SellPrice =   11000 },
        new() { FromLevel = 12, SuccessRate =  40f, EnhanceCost =  1900, SellPrice =   17000 },
        new() { FromLevel = 13, SuccessRate =  38f, EnhanceCost =  2500, SellPrice =   26000 },
        new() { FromLevel = 14, SuccessRate =  35f, EnhanceCost =  3200, SellPrice =   40000 },
        new() { FromLevel = 15, SuccessRate =  32f, EnhanceCost =  4200, SellPrice =   60000 },
        new() { FromLevel = 16, SuccessRate =  28f, EnhanceCost =  5500, SellPrice =   90000 },
        new() { FromLevel = 17, SuccessRate =  25f, EnhanceCost =  7000, SellPrice =  130000 },
        new() { FromLevel = 18, SuccessRate =  22f, EnhanceCost =  9000, SellPrice =  190000 },
        new() { FromLevel = 19, SuccessRate =  20f, EnhanceCost = 11500, SellPrice =  270000 },
        new() { FromLevel = 20, SuccessRate =  17f, EnhanceCost = 14500, SellPrice =  400000 },
        new() { FromLevel = 21, SuccessRate =  14f, EnhanceCost = 18000, SellPrice =  600000 },
        new() { FromLevel = 22, SuccessRate =  10f, EnhanceCost = 22000, SellPrice =  900000 },
        new() { FromLevel = 23, SuccessRate =   7f, EnhanceCost = 27000, SellPrice = 1400000 },
        new() { FromLevel = 24, SuccessRate =   3f, EnhanceCost = 35000, SellPrice = 2500000 },
    };

    // ── 대성공 기본 확률표 ───────────────────────────────────────────────────
    // 레벨 구간별 기본 대성공 확률입니다.
    // MinLevel 이상 MaxLevel 이하일 때 BaseRate가 적용됩니다.
    private static readonly GreatSuccessRateData[] GreatSuccessTable =
    {
        new() { MinLevel =  1, MaxLevel =  5, BaseRate = 10f },
        new() { MinLevel =  6, MaxLevel = 10, BaseRate =  5f },
        new() { MinLevel = 11, MaxLevel = 15, BaseRate =  3f },
        new() { MinLevel = 16, MaxLevel = 25, BaseRate =  1f },
    };

    private static readonly LastChanceRateData[] LastChanceTable =
    {
        new() { MinLevel =  1, MaxLevel =  10, BaseRate = 10f },
        new() { MinLevel =  11, MaxLevel = 20, BaseRate = 2f },
        new() { MinLevel = 21, MaxLevel = 24, BaseRate = 3f },
    };

    // ── 실패 드랍 테이블 ────────────────────────────────────────────────────
    [Header("실패 드랍 테이블")]
    [SerializeField] private FailDropData[] dropTable;

    public const int MAX_LEVEL = 25;
    public int CurrentLevel { get; private set; } = 1;

    [Header("참조")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject sellConfirmUI;

    // ── 아이템 효과 상태 ────────────────────────────────────────────────────
    // 확률 증가권: 다음 강화 1회에 한해 대성공 확률을 추가합니다.
    private float bonusGreatSuccessRate = 0f;

    // 파괴 방지권 적용 여부를 캐싱하는 ItemData 참조
    [Header("파괴 방지권 ItemData")]
    [Tooltip("파괴 방지권 ScriptableObject를 연결하세요. 자동 소모 감지에 사용됩니다.")]
    [SerializeField] private ItemData protectionScrollData;

    // ── 헬퍼 ────────────────────────────────────────────────────────────────
    private EnhanceLevelData? CurrentData =>
        CurrentLevel < MAX_LEVEL ? enhanceTable[CurrentLevel - 1] : null;

    public float CurrentSuccessRate => CurrentData?.SuccessRate ?? 0f;
    public int CurrentEnhanceCost => CurrentData?.EnhanceCost ?? 0;
    public int CurrentSellPrice => CurrentData?.SellPrice ?? enhanceTable[^1].SellPrice;
    public float BonusGreatSuccessRate => bonusGreatSuccessRate;

    /// <summary>
    /// 현재 레벨의 기본 대성공 확률을 반환합니다.
    /// </summary>
    public float BaseGreatSuccessRate
    {
        get
        {
            foreach (var entry in GreatSuccessTable)
            {
                if (CurrentLevel >= entry.MinLevel && CurrentLevel <= entry.MaxLevel)
                    return entry.BaseRate;
            }
            return 0f;
        }
    }

    public float LastChanceSuccessRate
    {
        get
        {
            foreach (var entry in LastChanceTable)
            {
                if (CurrentLevel >= entry.MinLevel && CurrentLevel <= entry.MaxLevel)
                    return entry.BaseRate;
            }
            return 0f;
        }
    }

    /// <summary>
    /// 실제 적용되는 최종 대성공 확률 (기본 + 확률 증가권 보너스)
    /// </summary>
    public float TotalGreatSuccessRate => BaseGreatSuccessRate + bonusGreatSuccessRate;

    private void Start()
    {
        sellConfirmUI.SetActive(false);
        RefreshDisplay();
    }

    // ── 강화 버튼 ───────────────────────────────────────────────────────────
    public void OnClickUpgrade()
    {
        if (CurrentLevel >= MAX_LEVEL)
        {
            Debug.Log("[강화] 최대 레벨입니다.");
            return;
        }
        if (!GameDataManager.gdmInstance.SpendMoney(CurrentEnhanceCost))
        {
            Debug.Log($"[강화] 재화 부족. 필요: {CurrentEnhanceCost}");
            return;
        }

        float roll = Random.Range(0f, 100f);
        float totalGreatRate = TotalGreatSuccessRate;

        // 1순위: 대성공 판정
        if (roll <= totalGreatRate)
        {
            OnGreatSuccess();
        }
        // 2순위: 일반 성공 판정
        else if (roll <= CurrentSuccessRate)
        {
            OnEnhanceSuccess();
        }
        // 3순위: 실패
        else
        {
            OnEnhanceFail();
        }

        // 확률 증가권 효과는 1회 사용 후 초기화
        bonusGreatSuccessRate = 0f;

        RefreshDisplay();
    }

    // ── 강화 결과 처리 ───────────────────────────────────────────────────────
    private void OnGreatSuccess()
    {
        int gainedLevels = Mathf.Min(2, MAX_LEVEL - CurrentLevel); // 최대 레벨 초과 방지
        CurrentLevel += gainedLevels;
        Debug.Log($"[대성공!] → Lv.{CurrentLevel} (+{gainedLevels})");
    }

    private void OnEnhanceSuccess()
    {
        CurrentLevel++;
        Debug.Log($"[강화 성공] → Lv.{CurrentLevel}");
    }

    private void OnEnhanceFail()
    {
        int failedLevel = CurrentLevel;
        float chanceRoll = Random.Range(0f, 100f);

        if (chanceRoll <= LastChanceSuccessRate)
        {
            Debug.Log($"[파괴 회피] Lv.{failedLevel} 강화 실패 무효화!");
            return; // 레벨 유지, 초기화 없음
        }

        // 파괴 방지권 보유 여부 확인
        else if (protectionScrollData != null &&
            ItemManager.imInstance.GetCount(protectionScrollData) > 0)
        {
            ItemManager.imInstance.RemoveItem(protectionScrollData, 1);
            Debug.Log($"[파괴 방지권 발동] Lv.{failedLevel} 강화 실패 무효화! (잔여: {ItemManager.imInstance.GetCount(protectionScrollData)}개)");
            return; // 레벨 유지, 초기화 없음
        }

        // 방지권 없을 때 일반 실패 처리
        Debug.Log($"[강화 실패] Lv.{failedLevel} → Lv.1");
        TryDropItem(failedLevel);
        CurrentLevel = 1;
    }

    // ── 드랍 처리 ───────────────────────────────────────────────────────────
    private void TryDropItem(int failedLevel)
    {
        FailDropData? bestDrop = null;
        foreach (var drop in dropTable)
        {
            if (failedLevel >= drop.RequiredLevel &&
                (bestDrop == null || drop.RequiredLevel > bestDrop.Value.RequiredLevel))
                bestDrop = drop;
        }
        if (bestDrop.HasValue && bestDrop.Value.DropItem != null)
            ItemManager.imInstance.AddItem(bestDrop.Value.DropItem, bestDrop.Value.DropAmount);
    }

    // ── 아이템 효과 API (ItemManager에서 호출) ───────────────────────────────

    /// <summary>확률 증가권: 다음 강화 1회에 대성공 확률을 추가합니다.</summary>
    public void ApplyProbabilityBoost(int boostAmount)
    {
        bonusGreatSuccessRate += boostAmount;
        Debug.Log($"[확률 증가권] 다음 강화 대성공 확률 +{boostAmount}% (현재: {bonusGreatSuccessRate}%)");
        RefreshDisplay();
    }

    /// <summary>즉시 성장권: 지정 레벨로 즉시 이동합니다. (10 또는 20)</summary>
    public void ApplyInstantGrowth(int targetLevel)
    {
        if (targetLevel <= 0 || targetLevel > MAX_LEVEL)
        {
            Debug.LogWarning($"[즉시 성장권] 유효하지 않은 목표 레벨: {targetLevel}");
            return;
        }
        int prevLevel = CurrentLevel;
        CurrentLevel = targetLevel;
        Debug.Log($"[즉시 성장권] Lv.{prevLevel} → Lv.{CurrentLevel}");
        RefreshDisplay();
    }

    /// <summary>룰렛권: 1~25 랜덤 레벨로 변경합니다.</summary>
    public void ApplyRoulette()
    {
        int prevLevel = CurrentLevel;
        CurrentLevel = Random.Range(1, MAX_LEVEL + 1);
        Debug.Log($"[룰렛권] Lv.{prevLevel} → Lv.{CurrentLevel} (랜덤)");
        RefreshDisplay();
    }

    // ── 판매 ────────────────────────────────────────────────────────────────
    public void OnClickSell() => sellConfirmUI.SetActive(true);
    public void OnClickSellCancel() => sellConfirmUI.SetActive(false);

    public void OnClickSellConfirm()
    {
        GameDataManager.gdmInstance.AddMoney(CurrentSellPrice);
        Debug.Log($"[판매] 판매가: {CurrentSellPrice}");
        CurrentLevel = 1;
        bonusGreatSuccessRate = 0f;
        sellConfirmUI.SetActive(false);
        RefreshDisplay();
    }

    private void RefreshDisplay() =>
        levelManager.UpdateDisplay(CurrentLevel, TotalGreatSuccessRate, CurrentSuccessRate, CurrentEnhanceCost, CurrentSellPrice);

    // ── 디버그 ───────────────────────────────
    public void OnClickDebugBtn()
    {
        CurrentLevel++;
        RefreshDisplay();
    }
}
