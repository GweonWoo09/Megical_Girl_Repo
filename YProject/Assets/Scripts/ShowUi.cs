using UnityEngine;

// ButtenManager 상속을 지우고 MonoBehaviour로 바꿉니다.
public class ShowUi : MonoBehaviour
{
    // 유니티 인스펙터창에서 진짜 레벨이 오르는 버튼 매니저를 연결할 칸을 만듭니다.
    [SerializeField] private ButtenManager realButtonManager;

    void Update()
    {
        ZFPrice();
    }

    public void ZFPrice()
    {
        // 연결된 버튼 매니저가 있고, 레벨이 1 이상일 때
        if (realButtonManager != null && realButtonManager.currentLevel >= 1)
        {
            // 진짜 레벨업이 일어나는 버튼 매니저의 레벨을 가져와서 곱합니다!
            PriceSystem.showprice(realButtonManager.currentLevel * 100);
        }
        else if (realButtonManager == null)
        {
            Debug.LogWarning("ShowUi: realButtonManager가 연결되지 않았습니다!");
        }
    }
}