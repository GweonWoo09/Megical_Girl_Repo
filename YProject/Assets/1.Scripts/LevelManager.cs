using UnityEngine;
using TMPro;

/// <summary>
/// [개선] EnhanceManager로부터 레벨/확률/비용/판매가를 받아 UI에 표시합니다.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI successRateText;  // 성공 확률 표시
    [SerializeField] private TextMeshProUGUI enhanceCostText;  // 강화 비용 표시
    [SerializeField] private TextMeshProUGUI sellPriceText;    // 판매 가격 표시

    private void Start()
    {
        UpdateDisplay(1, 100f, 50, 0);
    }

    /// <summary>
    /// EnhanceManager가 레벨 변화 시 호출합니다.
    /// </summary>
    public void UpdateDisplay(int level, float successRate, int enhanceCost, int sellPrice)
    {
        if (levelText != null)
            levelText.text = $"Level: {level}";

        if (nameText != null)
            nameText.text = GetCharacterName(level);

        // 최대 레벨이면 "MAX" 표시
        bool isMax = level >= EnhanceManager.MAX_LEVEL;

        if (successRateText != null)
            successRateText.text = isMax ? "MAX" : $"성공 확률: {successRate}%";

        if (enhanceCostText != null)
            enhanceCostText.text = isMax ? "-" : $"강화 비용: {enhanceCost:N0}G";

        if (sellPriceText != null)
            sellPriceText.text = $"판매 가격: {sellPrice:N0}G";

        // 가격 갱신
        GameDataManager.SetPrice(sellPrice);
    }

    private static string GetCharacterName(int level)
    {
        // 1~5 레벨은 고유 이름, 이후는 자동 생성
        return level switch
        {
            1 => "레벨1 마법소녀",
            2 => "레벨2 마법소녀",
            3 => "레벨3 마법소녀",
            4 => "레벨4 마법소녀",
            5 => "레벨5 마법소녀",
            _ => $"레벨{level} 마법소녀"
        };
    }
}