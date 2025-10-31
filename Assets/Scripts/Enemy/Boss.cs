using System.Collections;
using UnityEngine;

public abstract class Boss : Enemy
{
    [SerializeField] private float _regenCooldown;
    [SerializeField] private int _regenAmount;

    private void OnValidate()
    {
        _regenCooldown = Mathf.Clamp(_regenCooldown, 0, Mathf.Abs(_regenCooldown));
        _regenAmount = Mathf.Clamp(_regenAmount, 0, Mathf.Abs(_regenAmount));
    }
    
    private IEnumerator Regen()
    {
        _health = Mathf.Clamp(_health + _regenAmount, 0, _maxHealth);
        
        yield return new WaitForSeconds(_regenCooldown);
    }
    
    public abstract void Spare();

    public abstract void Kill();
}
