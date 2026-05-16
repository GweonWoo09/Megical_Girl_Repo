using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject targetUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
