using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperRound : Bullet
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // TODO Damage Enemy
        Destroy(gameObject);
    }
}
