using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtenManager : GameManager
{
    
    public float successRate = 100f; // 강화 성공 확률
    public int currentLevel = 1;
    public LevelUp levelUpScript;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public GameObject FindObject()
    {
        return GameObject.FindGameObjectWithTag("Zhuang Fangyi");
    }

    public void OnClickUpgrade()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= successRate)
        {
            currentLevel++;
            successRate -= 2f; // 강화 성공 확률을 감소시킵니다. (예: 매 강화마다 2%씩 감소)
            Debug.Log("강화 성공!");
            levelUpScript.LevelUpSystem(currentLevel);
        }
        else
        {
            Debug.Log("강화 실패!");
        }
    }

    public void OnClickSell()
    {
        targetUI.SetActive(true);
         
    }

    public void OnClickSellCheck()
    {
        if (currentLevel >= 1)
        {
            int sellPrice = currentLevel * 100; // 판매 가격을 레벨에 따라 계산합니다. (예: 레벨 x 100)
            GetMoney.getMoney(sellPrice);
            Debug.Log("아이템이 판매되었습니다. 판매 가격: " + sellPrice);
            currentLevel = 1; // 아이템 레벨을 초기화합니다.
            successRate = 100f; // 강화 성공 확률을 초기화합니다.
            levelUpScript.LevelUpSystem(currentLevel); // UI 업데이트
            targetUI.SetActive(false); // 판매 UI를 닫습니다.
        }
        else
        {
            Debug.Log("판매할 아이템이 없습니다.");
        }
    }

    public void OnClickSellCancel()
    {
        targetUI.SetActive(false);
    }
}
