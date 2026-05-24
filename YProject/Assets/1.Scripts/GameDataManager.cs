using System;
using UnityEngine;

/// <summary>
/// [통합] GetMoney + PriceSystem
/// 게임 내 재화와 가격 데이터를 한 곳에서 관리합니다.
/// 값이 바뀔 때만 이벤트를 발생시켜 불필요한 매 프레임 폴링을 제거합니다.
/// </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    // --- 이벤트 ---
    // PriceManager 등 다른 스크립트가 구독하면 값이 바뀔 때 자동으로 알림을 받습니다.
    public static event Action<int> OnMoneyChanged;
    public static event Action<int> OnPriceChanged;

    // --- 데이터 (외부에서 직접 수정 불가) ---
    public static int Money { get; private set; }
    public static int Price { get; private set; }

    private void Awake()
    {
        // 싱글턴 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Money = 0;
        Price = 0;
    }

    /// 재화 추가
    public static void AddMoney(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money); // 구독자에게 변경 알림
    }

    /// 재화 차감 (부족하면 false)
    public static bool SpendMoney(int amount)
    {
        if (Money < amount)
        {
            Debug.LogWarning($"재화 부족: 보유 {Money}, 필요 {amount}");
            return false;
        }
        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }

    /// 현재 아이템 가격 설정
    public static void SetPrice(int price)
    {
        if (Price == price) return; // 값이 같으면 이벤트 발생 안 함
        Price = price;
        OnPriceChanged?.Invoke(Price);
    }
}
