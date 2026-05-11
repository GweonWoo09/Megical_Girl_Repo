using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ProbabilityManager : MonoBehaviour
{
    public float probability = 0;

    void Start()
    {
        
    }
    
    public void ProbabilityCheck()
    {
        float randomProb = Random(0.0f, 1.0f);
        if (prob <= randomProb)
        {
            Debug.Log("강화 성공");
        }
        else
        {
            Debug.Log("강화 실패");
        }
    }
}
