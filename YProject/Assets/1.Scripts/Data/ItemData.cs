using UnityEngine;

/// <summary>
/// 아이템 원본 정보 ScriptableObject.
/// effectType으로 아이템 효과를 구분하고,
/// effectValue로 효과 수치(즉시 성장 목표 레벨, 확률 증가량 등)를 지정합니다.
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("아이템 효과")]
    public ItemEffectType effectType;

    [Tooltip(
        "파괴 방지권: 사용 안 함\n" +
        "확률 증가권: 대성공 확률 증가량 (%)\n" +
        "즉시 성장권: 이동할 목표 레벨 (10 또는 20)\n" + 
        "룰렛권: 사용 안 함\n" +
        "재화 획득권: 사용 안 함 (보유 개수가 레벨로 적용됨)")]
    public int effectValue;
}

public enum ItemEffectType
{
    None,
    ProtectionScroll,  // 파괴 방지권 (강화 실패 자동 소모, 피동)
    ProbabilityBoost,  // 확률 증가권 (사용 → 다음 강화에 대성공 확률 증가)
    EarnMoney,     // 돈벌기 아이템 (1초마다 돈이 +10씩 오름, 레벨 올릴수록 +10씩 더 추가)
    InstantGrowth,     // 즉시 성장권 (사용 → effectValue 레벨로 즉시 이동)
    Roulette,          // 룰렛권     (사용 → 1~25 랜덤 레벨)
}