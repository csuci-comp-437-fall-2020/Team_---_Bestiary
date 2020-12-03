
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat : MonoBehaviour
{
    public EnemyStats baseStats;
    public bool isAlive = true;
    public GameObject[] pickupPrefabs;
    [HideInInspector] public EnemyStats enemyStats;
    



    public abstract void TakeDamage(int damage);

    public abstract void HealDamage(int heal);

    public virtual void DropOnDeath()
    {
        if (Random.Range(0, 15) == 0)
        {
            Instantiate(pickupPrefabs[Random.Range(0, 100) % pickupPrefabs.Length], transform.position, Quaternion.identity);
        }
    }
}
