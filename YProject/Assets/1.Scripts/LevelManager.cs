using UnityEngine;
using TMPro;

/// <summary>
/// 레벨 표시와 가격 계산을 한 곳에서 처리, 
/// EnhanceManager가 레벨 바뀔 때만 호출
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI nameText;

    private const int PRICE_PER_LEVEL = 100;

    // 레벨별 캐릭터 이름
    private static readonly string[] CharacterNames =
    {
        "",             // index 0 (사용 안 함)
        "레벨1 마법소녀",
        "레벨2 마법소녀",
        "레벨3 마법소녀",
        "레벨4 마법소녀",
        "레벨5 마법소녀",
    };

    private void Start()
    {
        UpdateDisplay(1);
    }

    /// <summary>
    /// 레벨이 바뀔 때 EnhanceManager가 호출, UI 갱신 + 가격 갱신
    /// </summary>
    public void UpdateDisplay(int level)
    {
        // UI 갱신
        // 레벨 텍스트
        if (levelText != null)
            levelText.text = "Level: " + level;
        else
            Debug.LogError("levelText가 인스펙터에 연결되지 않았습니다!");

        // 캐릭터 이름 텍스트
        if (nameText != null)
        {
            string name = (level < CharacterNames.Length)
                ? CharacterNames[level]
                : $"레벨{level} 마법소녀"; // 배열 범위 초과 시 자동 생성
            nameText.text = name;
        }
        else
            Debug.LogError("NameText가 인스펙터에 연결되지 않았습니다!");

        // 가격 갱신
        int price = level * PRICE_PER_LEVEL;
        GameDataManager.SetPrice(price);
    }
}
