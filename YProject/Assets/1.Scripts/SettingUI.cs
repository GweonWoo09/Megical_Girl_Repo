using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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

        // 슬라이더 값이 바뀔 때마다 실시간으로 음량 적용
        volumeSlider.onValueChanged.AddListener(ApplyVolume);
    }

    private void Start()
    {
        // 저장된 볼륨값 불러오기 (없으면 DEFAULT_VOLUME 사용)
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, DEFAULT_VOLUME);
        volumeSlider.value = savedVolume;
        ApplyVolume(savedVolume); // 불러온 값을 즉시 오디오에 반영

        popSetting.SetActive(false);
    }

    public void OpenSetting() => popSetting.SetActive(true);

    /// <summary>
    /// 슬라이더가 움직일 때마다 실시간으로 오디오에 반영합니다.
    /// </summary>
    private void ApplyVolume(float value)
    {
        if (audioMixer != null)
        {
            // AudioMixer는 로그 스케일을 사용합니다.
            // 슬라이더 0~1 값을 -80dB~0dB로 변환합니다.
            float db = value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;
            audioMixer.SetFloat("BgmVolume", db); // AudioMixer의 파라미터 이름과 맞춰주세요
        }
        else
        {
            // AudioMixer 없이 AudioListener 전체 볼륨으로 조절합니다.
            AudioListener.volume = value;
        }
    }

    /// <summary>
    /// 닫기 버튼: 현재 슬라이더 값을 저장하고 패널을 닫습니다.
    /// </summary>
    public void SaveSettingData()
    {
        PlayerPrefs.SetFloat(VOLUME_KEY, volumeSlider.value);
        PlayerPrefs.Save(); // 즉시 디스크에 기록
        Debug.Log($"볼륨 저장 완료: {volumeSlider.value}");
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