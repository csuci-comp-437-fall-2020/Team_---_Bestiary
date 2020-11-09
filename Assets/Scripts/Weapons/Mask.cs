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
    public WeaponSlot slot;
    [HideInInspector] public PlayerMovement movement;

    public abstract void Fire(float damageMult, float fireRateMult);
}
