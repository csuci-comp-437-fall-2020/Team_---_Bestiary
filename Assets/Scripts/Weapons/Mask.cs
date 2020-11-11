using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mask : MonoBehaviour
{
    public enum WeaponSlot
    {
        Right1,
        Right2,
        Right3,
        Left1,
        Left2,
        Left3,
        Up1,
        Up2,
        Up3,
        Down1,
        Down2,
        Down3
    }

    public int powerLevel;
    public float knockback;
    public float damage;
    public float spread;
    public Bullet bulletPrefab;
    public WeaponSlot slot;
    [HideInInspector] public PlayerEffects playerEffects;
    [HideInInspector] public WeaponAim weaponAim;

    public abstract void Fire();
}
