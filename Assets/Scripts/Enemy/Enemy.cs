using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private const int MinGenHealth = 80;
    private const int MaxGenHealth = 120;
    
    protected int _health;
    protected int _maxHealth;
    
    public int Health => _health;
    public int MaxHealth => _maxHealth;

    public int MissingHealth => _maxHealth - _health;

    public event Action<int> Died;

    public void OnSpawn(int locMod, int id)
    {
        _maxHealth = Random.Range(MinGenHealth, MaxGenHealth) * locMod * Mathf.Max(1, locMod ^ id);
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= Mathf.Clamp(damage, 0, Mathf.Abs(damage));

        if (_health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Died?.Invoke(_maxHealth);
        Destroy(gameObject);
    }
}
