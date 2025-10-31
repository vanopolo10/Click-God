using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Vector2 _spawnPosition;
    
    private int _currentWaveID;
    private int _currentEnemyID;
    private Enemy _currentEnemy;

    private Vector3 _instantiatePosition;

    private void Start()
    {
        _instantiatePosition = new Vector3(_spawnPosition.x, _spawnPosition.y, 0);
    }

    private void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_waves[_currentWaveID].Enemies.Count == _currentEnemyID + 1
            ? _waves[_currentWaveID].Boss
            : _waves[_currentWaveID].Enemies[_currentEnemyID]
        ,_instantiatePosition, new Quaternion());

        _currentEnemy.Died += OnEnemyDead;
    }
    
    private void OnEnemyDead(int reward)
    {
        //Wallet.AddMoney

        _currentEnemy.Died -= OnEnemyDead;
    }
}

[Serializable]
public struct Wave
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private Boss _boss;

    public IReadOnlyList<Enemy> Enemies => _enemies;
    public Boss Boss => _boss;
}