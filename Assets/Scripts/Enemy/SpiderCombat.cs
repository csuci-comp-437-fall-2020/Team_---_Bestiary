﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCombat : Combat
{
    private Animator _animator;
    private EnemyAI _enemyAI;
    private Rigidbody2D _body;
    private static readonly int PlayDead = Animator.StringToHash("PlayDead");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Dead = Animator.StringToHash("Dead");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyAI = GetComponent<EnemyAI>();
        _body = GetComponent<Rigidbody2D>();
        enemyStats = Instantiate(baseStats);
        enemyStats.hitPoints = enemyStats.maxHitPoints;
    }

    public override void TakeDamage(int damage)
    {
        enemyStats.hitPoints -= damage;
        if (enemyStats.hitPoints <= 0)
        {
            isAlive = false;
            _animator.enabled = false;
            _body.gravityScale = 1;   
            StartCoroutine(Die());
        }
    }

    public override void HealDamage(int heal)
    {
        enemyStats.hitPoints = Math.Min(heal + enemyStats.hitPoints, enemyStats.maxHitPoints);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
