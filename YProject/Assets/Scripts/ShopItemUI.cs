using System;
using UnityEngine;
using UnityEngine.UI; // 👈 이 부분이 빠져서 Button 에러가 났던 것입니다!
using TMPro;

public class ShopItemUI : Gpm.Ui.InfiniteScrollItem
{
    [Header("UI References")]
    public TextMeshProUGUI NameAndDescText; // "아이템: 설명" 텍스트
    public Button BuyButton;

    private ShopItemData myData;

    private void Awake()
    {
        BuyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    public override void UpdateData(Gpm.Ui.InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        myData = scrollData as ShopItemData;

        if (myData != null)
        {
            NameAndDescText.text = $"{myData.ItemName}: {myData.ItemDescription}";
        }
    }

    private void OnBuyButtonClicked()
    {
        OnSelect();
    }
}
