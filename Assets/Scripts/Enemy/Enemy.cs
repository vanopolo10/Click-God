using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] private int _reward;
    
    protected int _maxHealth;

    public int Health => _health;

    public event Action<int> Died;

    private void OnValidate()
    {
        _maxHealth = _health;
    }

    public void TakeDamage(int damage)
    {
        _health -= Mathf.Clamp(damage, 0, Mathf.Abs(damage));

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(_reward);
        Destroy(gameObject);
    }
}
