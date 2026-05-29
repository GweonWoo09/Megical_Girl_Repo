using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Second Scene";

    public void SceneChange()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
