using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mountain : MonoBehaviour
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
    
    private int _totalDamageDealt = 0;
    
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

    private void BossDefeated()
    {
        _currentWaveID++;
    }
    
    private void Roll(int damage)
    {
        int clampedDamage = Mathf.Min(damage, _currentEnemy.Health);

        _totalDamageDealt += clampedDamage;
        _totalDamageDealt = Mathf.Clamp(_totalDamageDealt, 0, _currentEnemy.MaxHealth);

        float progress = _totalDamageDealt / (float)_currentEnemy.MaxHealth;

        _currentEnemy.transform.position = Vector3.Lerp(_instantiatePosition, _targetPosition, progress);

        print($"Враг сдвинут на {progress * 100:F1}% пути");

        _currentEnemy.TakeDamage(damage);
    }

    
    private void SpawnEnemy()
    {
        _totalDamageDealt = 0;
        
        if (_waves[_currentWaveID].EnemiesCount == _currentEnemyID)
        {
            _currentEnemy = Instantiate(_waves[_currentWaveID].Boss,_instantiatePosition, new Quaternion());
            StartBossFight();
        }
        else
        {
            _currentEnemy = Instantiate(_waves[_currentWaveID].EnemyPrefab,_instantiatePosition, new Quaternion());
            _currentEnemy.GetComponent<SpriteRenderer>().sprite = _waves[_currentWaveID]
                .Sprites[Random.Range(0, _waves[_currentWaveID].Sprites.Count)];
        }
        
        _currentEnemy.OnSpawn(_currentWaveID + 1, _currentEnemyID + 1);
        _currentEnemy.Died += OnEnemyDead;
    }

    private void StartBossFight()
    {
        bool isBossDefeated = false;

        if (isBossDefeated)
        {
            _currentWaveID++;
            _currentEnemyID = 0;
        }
        else
        {
            _currentEnemyID = 0;
        }
        
        SpawnEnemy();
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
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private List<Sprite> _enemiesSprites;
    [SerializeField] private Boss _boss;
    [SerializeField] private Sprite _background;

    public int EnemiesCount => 10;
    public Enemy EnemyPrefab => _enemyPrefab;
    public IReadOnlyList<Sprite> Sprites => _enemiesSprites;
    public Boss Boss => _boss;
    public Sprite Background => _background;
}   