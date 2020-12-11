using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnerLife;
    public GameObject prefab;
    public bool alive;

    private int _spawnsLeft;

    private void OnEnable()
    {
        alive = true;
        StartCoroutine(SpawnCreature(prefab));
        _spawnsLeft = spawnerLife;
    }

    // Update is called once per frame
    void Update()
    {
        if(_spawnsLeft <= 0)
        {
            alive = false;
            gameObject.SetActive(false);
            enabled = false;
        }
    }
    public IEnumerator SpawnCreature(GameObject creature)
    {
        while(alive)
        {
            yield return new WaitForSecondsRealtime(4.0f);
            GameObject newCreature = Instantiate(creature, transform.position, Quaternion.identity);
            newCreature.name = "Creature";
            _spawnsLeft--;
        }
    }
}
