using UnityEngine;
using TMPro;

public class PriceManager : MonoBehaviour
{
    public static PriceManager Instance { get; private set; }

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
        // 이벤트 구독: 값이 바뀔 때만 호출됨
        GameDataManager.OnMoneyChanged += RefreshMoneyUI;
        GameDataManager.OnPriceChanged += RefreshPriceUI;
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화되면 구독 해제
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
