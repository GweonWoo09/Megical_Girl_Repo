using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject SellUI;
    [SerializeField] public GameObject SettingUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SellUI.SetActive(false);
        SettingUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
