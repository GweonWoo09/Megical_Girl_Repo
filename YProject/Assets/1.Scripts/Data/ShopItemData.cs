/// <summary>
/// 상점 아이템 데이터
/// LinkedItem: ScriptableObject로 저장되어 인벤토리에 추가.
/// </summary>
[System.Serializable]

public class ShopItemData
{
    public string ItemName;
    public string ItemDescription;
    public int Price;
    public ItemData LinkedItem; // 인벤토리 연동용 ScriptableObject
}
