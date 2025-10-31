using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Boulder : MonoBehaviour
{
    [SerializeField] private int _damage;
    public event Action<int> Rolled;

    private void OnMouseDown()
    {
        print("Boulder Pressed!");
        Rolled?.Invoke(_damage);
    }
}
