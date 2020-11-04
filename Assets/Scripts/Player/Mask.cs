using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    
    public int extraJumps;
    public int extraDashes;
    public int slamPower;
    public float slamSpeed;
    public float wallClingTime;
    public float floatSpeed;
    public float floatDuration;

    public SpriteRenderer GetMask()
    {
        return GetComponent<SpriteRenderer>();
    }
}
