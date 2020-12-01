
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Combat : MonoBehaviour
{
    public EnemyStats baseStats;

    public abstract void TakeDamage(int damage);

    public abstract void HealDamage(int heal);
}
