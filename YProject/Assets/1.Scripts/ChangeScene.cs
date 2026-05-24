using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// [수정] 오타 수정: ChangScene → ChangeScene
/// </summary>
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Second Scene";

    public void SceneChange()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
