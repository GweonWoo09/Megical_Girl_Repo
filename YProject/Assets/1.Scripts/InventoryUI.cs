using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField] private GameObject InventPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [Header("스크롤뷰")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private ShopItemUI itemPrefab;

    void Start()
    {
        
    }
}
