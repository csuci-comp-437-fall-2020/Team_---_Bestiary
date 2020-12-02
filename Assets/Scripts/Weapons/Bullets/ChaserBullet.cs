using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // TODO Damage Enemy
        Destroy(gameObject);
    }

    private void Update()
    {
        throw new NotImplementedException();
    }
}
