using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int target = 60;
    public GameObject spawnerPrefab;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
#endif
    }
    
    void Update()
    {
        if(Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Spawn()
    {
        spawnerPrefab.SetActive(true);
    }

}
