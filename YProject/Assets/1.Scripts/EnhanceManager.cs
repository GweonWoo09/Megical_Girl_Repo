using UnityEngine;

/// <summary>
/// 강화/판매 로직 담당, 
/// 가격 계산은 LevelManager에게
/// </summary>
public class EnhanceManager : MonoBehaviour
{
    // ── 강화 테이블 ──────────────────────────────────
    // FromLevel | SuccessRate | EnhanceCost | SellPrice
    // (해당 레벨) (강화 확률)   (강화 비용)  (판매 가격)
    private static readonly EnhanceLevelData[] Table =
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

    public const int MAX_LEVEL = 25; // 최대 레벨

    public int CurrentLevel { get; private set; } = 1; //현재 상태

    [Header("강화 설정")]
    private const int MIN_SELL_POPUP_LEVEL = 15; // 판매 확인 팝업을 여는 최소 레벨

    [Header("참조")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject sellConfirmUI; // 판매 확인 팝업
    [SerializeField] private GameObject btnCheatDebug; // 디버그용 버튼

    // ── 현재 레벨의 테이블 데이터를 가져오는 헬퍼 ──────────────────────────
    /// <summary>현재 레벨에서 강화 시도 시 적용될 데이터. 최대 레벨이면 null.</summary>
    private EnhanceLevelData? CurrentData =>
        CurrentLevel < MAX_LEVEL ? Table[CurrentLevel - 1] : null;

    public float CurrentSuccessRate => CurrentData?.SuccessRate ?? 0f;
    public int CurrentEnhanceCost => CurrentData?.EnhanceCost ?? 0;
    public int CurrentSellPrice => CurrentData?.SellPrice ?? Table[^1].SellPrice;

    private void Start()
    {
#if UNITY_EDITOR
        btnCheatDebug.SetActive(true);
#else
        btnCheatDebug.SetActive(false);
#endif
        sellConfirmUI.SetActive(false);
        levelManager.UpdateDisplay(CurrentLevel, CurrentSuccessRate, CurrentEnhanceCost, CurrentSellPrice);
    }

    // ── 강화 버튼 ───────────────────────────────────────────────────────────
    public void OnClickUpgrade()
    {
        // 최대 레벨 도달 시 강화 불가
        if (CurrentLevel >= MAX_LEVEL)
        {
            Debug.Log("[강화] 최대 레벨입니다.");
            return;
        }

        // 강화 비용 차감 시도
        if (!GameDataManager.SpendMoney(CurrentEnhanceCost))
        {
            // TODO: 재화 부족 UI 표시
            return;
        }

        float roll = Random.Range(0f, 100f);

        if (roll <= CurrentSuccessRate)
        {
            CurrentLevel++;
            Debug.Log($"[강화 성공] Lv.{CurrentLevel - 1} → Lv.{CurrentLevel}");
        }
        else
        {
            Debug.Log($"[강화 실패] Lv.{CurrentLevel} → Lv.1");
            CurrentLevel = 1;
        }

        levelManager.UpdateDisplay(CurrentLevel, CurrentSuccessRate, CurrentEnhanceCost, CurrentSellPrice);
    }

    // ── 판매 버튼 (확인 팝업 열기) ──────────────────────────────────────────
    public void OnClickSell()
    {
        if (CurrentLevel >= MIN_SELL_POPUP_LEVEL)
            sellConfirmUI.SetActive(true);
        else 
            OnClickSellConfirm();
    }

    // ── 판매 확정 ───────────────────────────────────────────────────────────
    public void OnClickSellConfirm()
    {
        int price = CurrentSellPrice;
        GameDataManager.AddMoney(price);
        Debug.Log($"[판매] 판매 완료. 판매가: {price}");

        ResetItem();
        sellConfirmUI.SetActive(false);
    }

    // ── 판매 취소 ───────────────────────────────────────────────────────────
    public void OnClickSellCancel()
    {
        sellConfirmUI.SetActive(false);
    }

    // ── 내부: 아이템 초기화 ─────────────────────────────────────────────────
    private void ResetItem()
    {
        CurrentLevel = 1;
        levelManager.UpdateDisplay(CurrentLevel, CurrentSuccessRate, CurrentEnhanceCost, CurrentSellPrice);
    }

    // ── 디버그 ───────────────────────────────
    public void OnClickCheatButton()
    {
        CurrentLevel++;
        Debug.Log($"[강화 성공] Lv.{CurrentLevel - 1} → Lv.{CurrentLevel}");
        levelManager.UpdateDisplay(CurrentLevel, CurrentSuccessRate, CurrentEnhanceCost, CurrentSellPrice);
    }
}
