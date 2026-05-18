using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PriceSystem : MonoBehaviour
{
    public static int currnetprice { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currnetprice = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void showprice(int getprice)
    {
        currnetprice = getprice;
        UIManager.Instance.showcurrentPrice();
    }

}
