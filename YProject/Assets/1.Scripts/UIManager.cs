using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI name_text;
    [SerializeField] private TextMeshProUGUI level_text;
    [SerializeField] private TextMeshProUGUI HaveMoney_text;
    [SerializeField] private TextMeshProUGUI Price_text;

    [SerializeField] private CharacterBase _character;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _character = GetComponent<CharacterBase>();
        if (_character == null)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        showCurrentName();
        showCurrentLevel();
        showCurrentPrice();
    }

    public void currentMoney()
    {
        HaveMoney_text.text = GetMoney.money.ToString();
    }

    public void showCurrentPrice()
    {
        Price_text.text = "가격: " + PriceSystem.currnetprice.ToString();
    }

    public void showCurrentName()
    {
        name_text.text = _character.charName.ToString();
    }

    public void showCurrentLevel()
    {
        level_text.text = _character.charLevel.ToString();
    }
}
