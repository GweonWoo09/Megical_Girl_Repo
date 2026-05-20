using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject targetUI;

    public int curLevel;

    private CharacterBase _character;

    private void Awake()
    {
        curLevel = 1;
    }

    void Start()
    {
        targetUI.SetActive(false);
        _character = GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
