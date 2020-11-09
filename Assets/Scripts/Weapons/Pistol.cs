using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Pistol : Mask
{
    [SerializeField] private float shotFrequency = 0.35f;
    
    private float _shotCooldown = 0;

    private void OnEnable()
    {
        _shotCooldown = 0;
    }

    public override void Fire(float damageMult, float fireRateMult)
    {
        if (_shotCooldown <= 0)
        {
            Debug.Log("Fired :)");
            _shotCooldown = shotFrequency * (1f / fireRateMult);
        }
            
    }
}
