using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject targetUI;

    public int curLevel;

    private CharacterBase character;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetUI.SetActive(false);
        character = GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
