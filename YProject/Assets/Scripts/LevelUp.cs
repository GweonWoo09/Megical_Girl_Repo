using UnityEngine;
using TMPro;
using UnityEngine.UI;// Text를 쓰려면 이게 꼭 있어야 합니다.

public class LevelUp : MonoBehaviour
{
    // 유니티 인스펙터에서 ZFLevel 이라는 Text 오브젝트를 여기에 드래그해서 넣으세요!
    public TextMeshProUGUI ZFLevelText;

    // 게임 시작 시 초기 상태 표시
    void Start()
    {
        LevelUpSystem(1);
    }

    void Update()
    {
        // 필요하다면 여기에 다른 업데이트 로직을 추가할 수 있습니다.
    }

    // 이 함수가 호출될 때만 글자가 바뀝니다.
    public void LevelUpSystem(int level)
    {
        if (ZFLevelText != null)
        {
            ZFLevelText.text = "Level: " + level.ToString();
        }
        else
        {
            Debug.LogError("ZFLevel Text가 할당되지 않았습니다! 인스펙터를 확인하세요.");
        }
    }
}