using UnityEngine;

/// <summary>
/// 아이템 원본 정보를 담는 ScriptableObject
/// Project 창에서 우클릭 → Create → Items → ItemData 로 생성
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("분류")]
    public ItemType itemType;
}

public enum ItemType
{
    Material,   // 재료 (마법 파편, 결정 등)
    Consumable, // 소비 아이템 (방지권 등)
    Equipment,  // 장비
}