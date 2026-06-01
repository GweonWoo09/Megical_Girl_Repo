using UnityEngine;

/// <summary>
/// 강화/판매 로직 담당, 
/// 가격 계산은 LevelManager에게
/// </summary>
public class EnhanceManager : MonoBehaviour
{
    [Header("강화 설정")]
    [Range(0f, 100f)]
    public float successRate = 100f;    // 강화 성공 확률 (%)
    public int currentLevel = 1;
    private float RATE_DECREASE = 10f;  // 강화 성공 시 확률 감소량
    private const int SELL_PRICE_PER_LEVEL = 100; // 레벨당 판매가
    private const int MIN_SELL_POPUP_LEVEL = 15; // 판매 확인 팝업을 여는 최소 레벨

    [Header("참조")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject sellConfirmUI; // 판매 확인 팝업
    [SerializeField] private GameObject btnCheatDebug; // 디버그용 버튼

    private void Start()
    {
#if UNITY_EDITOR
        sellConfirmUI.SetActive(true);
#else
        sellConfirmUI.SetActive(false);
#endif
        sellConfirmUI?.SetActive(false);
        levelManager?.UpdateDisplay(currentLevel);
    }

    // ── 강화 버튼 ──────────────────────────────
    public void OnClickUpgrade()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= successRate)
        {
            currentLevel++;
            probablity();
            Debug.Log($"강화 성공! Lv.{currentLevel} / 다음 성공 확률: {successRate}%");
        }
        else
        {
            Debug.Log("강화 실패!");
            currentLevel = 1;
            successRate = 100f;
        }

        levelManager.UpdateDisplay(currentLevel);
    }

    private void probablity()
    {
        if (currentLevel > 2 || currentLevel < 14) RATE_DECREASE = 5f;
        
        successRate = Mathf.Max(0f, successRate - RATE_DECREASE); // 0% 아래로 내려가지 않음
    }

    // ── 판매 버튼 (확인 팝업 열기) ──────────────
    public void OnClickSell()
    {
        if(currentLevel >= MIN_SELL_POPUP_LEVEL)
        {
            sellConfirmUI.SetActive(true);
        }
        else
        {
            OnClickSellConfirm();
        }
    }

    // ── 판매 확정 ───────────────────────────────
    public void OnClickSellConfirm()
    {
        int sellPrice = (currentLevel - 1) * SELL_PRICE_PER_LEVEL;
        GameDataManager.AddMoney(sellPrice);
        Debug.Log($"아이템 판매 완료. 판매가: {sellPrice}");

        ResetItem();
        sellConfirmUI.SetActive(false);
    }

    // ── 판매 취소 ───────────────────────────────
    public void OnClickSellCancel()
    {
        sellConfirmUI.SetActive(false);
    }

    // ── 내부: 아이템 초기화 ─────────────────────
    private void ResetItem()
    {
        currentLevel = 1;
        successRate = 100f;
        levelManager.UpdateDisplay(currentLevel);
    }

    // ── 디버그 ───────────────────────────────
    public void OnClickCheatButton()
    {
        currentLevel++;
    }
}
