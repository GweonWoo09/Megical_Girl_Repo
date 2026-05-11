using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GetMoney : MonoBehaviour
{
    public static int money {  get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void getMoney(int income)
    {
        money += income;
        UIManager.Instance.currentMoney();
    }
}
