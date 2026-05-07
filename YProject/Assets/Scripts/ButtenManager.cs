using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtenManager : MonoBehaviour
{
    
    private float successRate = 50f; // 강화 성공 확률
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

    private void OnClickUpgrade()
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= successRate)
        {
            currentLevel++;
            Debug.Log("강화 성공!");
            levelUpScript.LevelUpSystem(currentLevel);
        }
        else
        {
            Debug.Log("강화 실패!");
        }
    }

    private void OnClickSell()
    {
        GameObject ItemSell = FindObject();
        if (ItemSell == ItemSell.CompareTag("Zhuang Fangyi"))
        {   
            Debug.Log("판매 완료!");
            GetMoney.getMoney(100); // 판매 시 얻는 돈의 양을 조절할 수 있습니다. 

        }
        else
        {
            Debug.Log("판매할 아이템이 없습니다.");
        }
    }
}
