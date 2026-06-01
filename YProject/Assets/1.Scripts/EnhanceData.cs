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
