/// <summary>
/// 강화 테이블의 한 행을 나타내는 데이터 구조체
/// </summary>
[System.Serializable]
public struct EnhanceLevelData
{
    public int   FromLevel;      // 현재 레벨
    public float SuccessRate;    // 강화 성공 확률 (%)
    public int   EnhanceCost;    // 강화 비용
    public int   SellPrice;      // 판매 가격
}

/// <summary>
/// 강화 실패 시 드랍 조건과 아이템 이름 구조체
/// </summary>
[System.Serializable]
public struct ItemDropData
{
    public int RequiredLevel; // 이 레벨 이상에서 실패 시 드랍
    public string ItemName;
}
