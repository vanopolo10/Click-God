using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _money = 0;

    public int Money => _money;

    public event Action<int> OnMoneyChange;
    public bool TrySpendMoney(int value)
    {
        if (_money - value < 0 || value <= 0)
        {
            return false;
        }
        else
        {
            _money -= value;
            OnMoneyChange?.Invoke(_money);
            return true;
        }
    }

    public void AddMoney(int value)
    {
        _money += (int)Mathf.Clamp(value, 0, Mathf.Abs(value));
        OnMoneyChange?.Invoke(_money);
    }
}