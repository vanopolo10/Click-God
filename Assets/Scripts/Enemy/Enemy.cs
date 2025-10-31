using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _reward;
    
    public event Action<int> Died;
    
    public void TakeDamage(int damage)
    {
        _health -= Mathf.Clamp(damage, 0, Mathf.Abs(damage));

        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(_reward);
    }
}
