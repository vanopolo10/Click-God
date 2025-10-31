using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private Vector2 _spawnPosition;
    [SerializeField] private Vector2 _rollDirection;
    
    [Header("Character")]
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Boulder _boulder;
    
    [Header("Waves")]
    [SerializeField] private List<Wave> _waves;
    
    private int _currentWaveID;
    private int _currentEnemyID;
    private Enemy _currentEnemy;

    private Vector2 _targetPosition;
    private Vector3 _instantiatePosition;
    
    private void Start()
    {
        _targetPosition = _boulder.transform.position;
        _instantiatePosition = new Vector3(_spawnPosition.x, _spawnPosition.y, 0);
        
        SpawnEnemy();
    }

    private void OnEnable()
    {
        _boulder.Rolled += Roll;
    }
    
    private void OnDisable()
    {
        _boulder.Rolled -= Roll;
    }
    
    private void Roll(int distance)
    {
        print(_currentEnemy.Health / (float)distance);
        
        float distanceToMove = (float)distance / _currentEnemy.Health *
                               Vector2.Distance(_currentEnemy.transform.position, _targetPosition);
        
        _currentEnemy.TakeDamage(distance);
        print("Roll!");
        
        _currentEnemy.transform.position = Vector3.MoveTowards(
            _currentEnemy.transform.position, _targetPosition, distanceToMove);
    }
    
    private void SpawnEnemy()
    {
        _currentEnemy = Instantiate(_waves[_currentWaveID].Enemies.Count == _currentEnemyID
            ? _waves[_currentWaveID].Boss
            : _waves[_currentWaveID].Enemies[_currentEnemyID]
        ,_instantiatePosition, new Quaternion());
        
        _currentEnemy.Died += OnEnemyDead;
    }
    
    private void OnEnemyDead(int reward)
    {
        _wallet.AddMoney(reward);

        _currentEnemy.Died -= OnEnemyDead;
        _currentEnemyID++;
        SpawnEnemy();
    }
}

[Serializable]
public struct Wave
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private Boss _boss;
    [SerializeField] private Sprite _background;

    public IReadOnlyList<Enemy> Enemies => _enemies;
    public Boss Boss => _boss;
    public Sprite Background => _background;
}   