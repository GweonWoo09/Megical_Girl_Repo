using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [Header("UI 패널")]
    [SerializeField] private GameObject popSetting;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider volumeSlider;

    [Header("오디오")]
    [SerializeField] private AudioMixer audioMixer;

    private const string VOLUME_KEY = "BgmVolume";
    private const float DEFAULT_VOLUME = 0.5f;

    private void Awake()
    {
        openButton?.onClick.AddListener(OpenSetting);
        closeButton?.onClick.AddListener(SaveSettingData);

        volumeSlider.onValueChanged.AddListener(ApplyVolume);
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, DEFAULT_VOLUME);
        volumeSlider.value = savedVolume;
        ApplyVolume(savedVolume);

        popSetting.SetActive(false);
    }

    public void OpenSetting() => popSetting.SetActive(true);

    // 슬라이더가 움직일 때마다 실시간으로 오디오에 반영
    private void ApplyVolume(float value)
    {
        if (audioMixer != null)
        {
            // 슬라이더 0~1 값 = -80dB~0dB
            float db = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
            audioMixer.SetFloat("BgmVolume", db); // AudioMixer의 파라미터 이름과 맞추기
        }
        else
        {
            AudioListener.volume = value;
        }
    }

    public void SaveSettingData()
    {
        PlayerPrefs.SetFloat(VOLUME_KEY, volumeSlider.value);
        PlayerPrefs.Save();
        Debug.Log("Save");
        popSetting.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
