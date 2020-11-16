using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [HideInInspector] public int reflectCount;
    [HideInInspector] public int damage;
    [HideInInspector] public float lifetime;
}
