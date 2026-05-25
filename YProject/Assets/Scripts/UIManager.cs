using UnityEngine;
using TMPro;

/// <summary>
/// [개선] Update() 폴링 제거 → GameDataManager 이벤트 구독 방식으로 변경
/// 값이 실제로 바뀔 때만 UI를 업데이트합니다.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        // 이벤트 구독: 값이 바뀔 때만 호출됨 (매 프레임 호출 X)
        GameDataManager.OnMoneyChanged += RefreshMoneyUI;
        GameDataManager.OnPriceChanged += RefreshPriceUI;
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화되면 구독 해제 (메모리 누수 방지)
        GameDataManager.OnMoneyChanged -= RefreshMoneyUI;
        GameDataManager.OnPriceChanged -= RefreshPriceUI;
    }

    private void Start()
    {
        // 시작 시 초기값으로 UI 갱신
        RefreshMoneyUI(GameDataManager.Money);
        RefreshPriceUI(GameDataManager.Price);
    }

    private void RefreshMoneyUI(int money)
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    private void RefreshPriceUI(int price)
    {
        if (priceText != null)
            priceText.text = "가격: " + price.ToString();
    }
}
