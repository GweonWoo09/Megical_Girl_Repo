using UnityEngine;

public class ShowUi : ButtenManager
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ZFPrice();
    }

    public void ZFPrice()
    {
        GameObject ItemSell = FindObject();
        if (ItemSell == ItemSell.CompareTag("Zhuang Fangyi"))
        {
            PriceSystem.showprice(100);

        }
        else
        {
            Debug.Log("판매할 아이템이 없습니다.");
        }
         // 아이템의 가격을 100으로 설정합니다. 필요에 따라 조절할 수 있습니다.
    }
}
