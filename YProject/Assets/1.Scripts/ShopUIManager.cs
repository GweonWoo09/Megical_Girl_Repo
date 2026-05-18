using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 유니티 6 표준 텍스트 컴포넌트

#region 1. 코어 베이스 클래스 (이전 제공 코드 통합 및 최적화)
namespace Gpm.Ui
{
    public class InfiniteScrollData
    {
        // 베이스 데이터 클래스
    }

    public abstract class InfiniteScrollItem : MonoBehaviour
    {
        protected bool activeItem;
        protected InfiniteScrollData scrollData = null;
        protected Action<InfiniteScrollData> selectCallback = null;

        public bool IsActive => activeItem;

        public void AddSelectCallback(Action<InfiniteScrollData> callback) => selectCallback += callback;
        public void RemoveSelectCallback(Action<InfiniteScrollData> callback) => selectCallback -= callback;

        public virtual void UpdateData(InfiniteScrollData scrollData)
        {
            this.scrollData = scrollData;
        }

        protected void OnSelect()
        {
            selectCallback?.Invoke(scrollData);
        }

        public virtual void SetActive(bool active)
        {
            activeItem = active;
            if (gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
#endregion

#region 2. 상점 전용 데이터 및 UI 아이템 클래스
// 상점 아이템에 들어갈 실제 데이터
public class ShopItemData : Gpm.Ui.InfiniteScrollData
{
    public string ItemName;
    public string ItemDescription;
    public int Price;
}

// ⭐ 바로 이 부분입니다! 하나의 파일 안에 이 클래스가 이미 정의되어 있어요.

#endregion

#region 3. 상점 매니저 클래스 (최적화된 풀링 및 스크롤 관리)
public class ShopUIManager : MonoBehaviour
{
    [Header("Shop UI Panels")]
    public GameObject ShopPanel;      // 상점 팝업 전체 패널
    public Button OpenShopButton;     // 메인 화면의 상점 열기 버튼
    public Button CloseShopButton;    // 상점 안의 나가기 버튼

    [Header("Scroll View Setup")]
    public Transform ContentParent;   // Scroll View의 Content 오브젝트
    public ShopItemUI ItemPrefab;     // 상점 아이템 프리팹 (ShopItemUI 스크립트가 붙어있어야 함)

    // 내부 데이터 및 풀링 리스트
    private List<ShopItemData> shopItemDatas = new List<ShopItemData>();
    private List<ShopItemUI> spawnedItems = new List<ShopItemUI>();

    private void Start()
    {
        // 1. 버튼 이벤트 연결
        if (OpenShopButton != null) OpenShopButton.onClick.AddListener(OpenShop);
        if (CloseShopButton != null) CloseShopButton.onClick.AddListener(CloseShop);

        // 2. 임시 상점 데이터 생성 (요청하신 "방지권" 포함)
        CreateTemporaryData();

        // 3. 시작 시 상점 닫아두기
        CloseShop();
    }

    private void CreateTemporaryData()
    {
        shopItemDatas.Clear();

        // 임시 아이템 1: 요청하신 방지권
        shopItemDatas.Add(new ShopItemData
        {
            ItemName = "방지권",
            ItemDescription = "강화 실패 시 파괴를 1회 방지합니다.",
            Price = 100
        });

        // 리스트가 스크롤되는지 테스트하기 위한 더미 데이터 10개 추가
        for (int i = 1; i <= 10; i++)
        {
            shopItemDatas.Add(new ShopItemData
            {
                ItemName = $"일반 아이템 {i}",
                ItemDescription = $"테스트용 아이템 설명입니다.",
                Price = i * 10
            });
        }
    }

    public void OpenShop()
    {
        ShopPanel.SetActive(true);
        RefreshScrollView();
    }

    public void CloseShop()
    {
        ShopPanel.SetActive(false);
    }

    // 데이터에 맞춰 UI 아이템을 생성하거나 재사용(Pooling)합니다.
    private void RefreshScrollView()
    {
        // 필요한 만큼 아이템을 활성화하거나 새로 생성
        for (int i = 0; i < shopItemDatas.Count; i++)
        {
            ShopItemUI itemUI;

            if (i < spawnedItems.Count)
            {
                // 이미 생성된 UI가 있다면 재사용 (오브젝트 풀링)
                itemUI = spawnedItems[i];
            }
            else
            {
                // 모자라면 새로 생성
                itemUI = Instantiate(ItemPrefab, ContentParent);
                itemUI.AddSelectCallback(OnItemPurchased); // 구매 콜백 연결
                spawnedItems.Add(itemUI);
            }

            itemUI.SetActive(true);
            itemUI.UpdateData(shopItemDatas[i]);
        }

        // 데이터 개수보다 많이 생성되어 있는 남은 UI들은 비활성화 (숨김)
        for (int i = shopItemDatas.Count; i < spawnedItems.Count; i++)
        {
            spawnedItems[i].SetActive(false);
        }
    }

    // 아이템의 '구매' 버튼이 눌렸을 때 실행될 로직
    private void OnItemPurchased(Gpm.Ui.InfiniteScrollData data)
    {
        var itemData = data as ShopItemData;
        if (itemData != null)
        {
            Debug.Log($"[상점] '{itemData.ItemName}' 구매 시도! 가격: {itemData.Price}");
            // TODO: 재화 차감 및 인벤토리 추가 로직 작성
        }
    }
}
#endregion