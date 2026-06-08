using UnityEngine;

/// <summary>강화 테이블 한 행의 데이터입니다.</summary>
[System.Serializable]
public struct EnhanceLevelData
{
    public int FromLevel;
    public float SuccessRate;
    public int EnhanceCost;
    public int SellPrice;
}

/// <summary>
/// 강화 실패 시 드랍 조건과 지급할 ItemData를 정의합니다.
/// ItemData는 ScriptableObject이므로 인스펙터에서 직접 연결합니다.
/// </summary>
[System.Serializable]
public struct FailDropData
{
    public int RequiredLevel; // 이 레벨 이상 실패 시 드랍
    public ItemData DropItem;      // 지급할 아이템 (ScriptableObject)
    public int DropAmount;    // 지급 수량
}