using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int spawnerLife;
    public GameObject prefab;
    public bool alive;

    // Start is called before the first frame update
    void Start()
    {
        spawnerLife = 3;
        alive = true;
        StartCoroutine(SpawnCreature(prefab));
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnerLife <= 0)
        {
            alive = false;
            Destroy(gameObject);
        }
    }
    public IEnumerator SpawnCreature(GameObject creature)
    {
        while(alive)
        {
            yield return new WaitForSecondsRealtime(4.0f);
            GameObject newCreature = Instantiate(creature, transform.position, Quaternion.identity);
            newCreature.name = "Creature";
            spawnerLife--;
        }
    }
}
