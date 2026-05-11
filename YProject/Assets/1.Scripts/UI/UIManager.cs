using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI HaveMoney_text;
    public TextMeshProUGUI Price_text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void currentMoney()
    {
        HaveMoney_text.text = GetMoney.money.ToString();
    }

    public void showcurrentPrice()
    {
        Price_text.text = "가격: " + PriceSystem.currnetprice.ToString();
    }

    
}
