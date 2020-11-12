using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillCount : MonoBehaviour
{
    public int killcount;
    [SerializeField] private GameObject spawnerObj;
    private EnemySpawner spawnerScript;

    void Start()
    {
        killcount = 10;
        spawnerScript = spawnerObj.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if(spawnerScript !=null)
        {
            killcount = spawnerScript.spawnerLife;
        }
    }
}
