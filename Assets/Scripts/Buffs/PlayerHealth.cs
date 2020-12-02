using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerEffects playerEffects;
    
    private CircleCollider2D _hurtbox;

    [HideInInspector] public bool isAlive = true;

    private void Start()
    {
        _hurtbox = GetComponentInChildren<CircleCollider2D>();
    }

    public void TakeDamage(int damage)
    {
        playerEffects.hitPoints -= damage;
        if (playerEffects.hitPoints <= 0)
        {
            isAlive = false;
            Debug.Log("You are DEAD!");
        }

        StartCoroutine(ActivateIFrames(1.5f));
    }

    public IEnumerator ActivateIFrames(float duration)
    {
        _hurtbox.enabled = false;
        yield return new WaitForSeconds(duration);
        _hurtbox.enabled = true;
    }

    public void HealDamage(int heal)
    {
        playerEffects.hitPoints = Math.Min(heal + playerEffects.hitPoints, playerEffects.effectiveHitPoints);
    }

    public void RecalculateHitPoints(bool setHpToMax)
    {
        float eHP = 100 / (100 - playerEffects.damageResistance);
        playerEffects.effectiveHitPoints = Mathf.CeilToInt(playerEffects.maxHitPoints * eHP);
        if (setHpToMax)
        {
            playerEffects.hitPoints = playerEffects.effectiveHitPoints;
        }
        else
        {
            playerEffects.hitPoints = Mathf.CeilToInt(playerEffects.hitPoints * eHP);
        }
    }
}
