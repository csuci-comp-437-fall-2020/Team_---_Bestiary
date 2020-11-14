using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZantetsuSlash : Bullet
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            BatCombat batCombat = other.gameObject.GetComponent<BatCombat>();
            batCombat.TakeDamage(damage);
        }
    }

    private void Update()
    {
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
        lifetime -= Time.deltaTime;
    }
}
