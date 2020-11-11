using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Effects")]
public class PlayerEffects : ScriptableObject
{
    public float jumpHeightMult = 1f;
    public float moveSpeedMult = 1f;
    public int extraJumps;
    public int extraDashes;
    public float wallClingTime;
    public float floatDuration;
    [HideInInspector] public int slamPower;
    [HideInInspector] public int slamEquipped;
    public float gravityScale = 1f;
    public float damageMult = 1f;
    public float fireRateMult = 1f;
    public float knockbackMult = 1f;
}
