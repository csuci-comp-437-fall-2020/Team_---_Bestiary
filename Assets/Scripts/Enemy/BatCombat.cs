using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BatCombat : MonoBehaviour
{
    public EnemyStats baseStats;

    private Animator _animator;
    private bool _isAlive = true;
    private EnemyStats _enemyStats;
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
        _enemyStats = Instantiate(baseStats);
        _enemyStats.hitPoints = _enemyStats.maxHitPoints;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "PlayerHurtbox":
                if (_isAlive)
                {
                    PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
                    playerHealth.TakeDamage(_enemyStats.damage);                    
                }
                break;
            case "Platform":
            case "Floor":
                if (!_isAlive)
                    _animator.SetTrigger(PlayDead);
                break;
                
        }
    }

    public void TakeDamage(int damage)
    {
        _animator.SetTrigger(Hit);
        _enemyStats.hitPoints -= damage;
        if (_enemyStats.hitPoints <= 0)
        {
            _isAlive = false;
            _enemyAI.enabled = false;
            _body.gravityScale = 1;
            _animator.SetBool(Dead, true);
            StartCoroutine(Die());
        }
    }

    public void HealDamage(int heal)
    {
        _enemyStats.hitPoints = Math.Min(heal + _enemyStats.hitPoints, _enemyStats.maxHitPoints);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
