using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpawnerBomb : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    private int _maxBombs = 10;
    private int _minBombs = 3;
    private int _totalBombs;
    private float _tileSize = 1f; 
    private Vector3 _boardCenter = new Vector3(3.5f, 0f, 3.5f);
    private Transform _bombs;
    private float _timeSpawn = 2f;
    private float _timer;
    private bool _isSpawning;

    private void Start()
    {
        _timer = _timeSpawn;
        _bombs = new GameObject("Bombs").transform;
    }

    private void Update()
    {
        if (!_isSpawning)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = _timeSpawn;
                GenerateRandomBombCount();
                SpawnBombs();
                _isSpawning = true;
            }
        }
    }

    void GenerateRandomBombCount()
    {
        _minBombs = Mathf.Max(0, _minBombs);
        _maxBombs = Mathf.Min(64, _maxBombs);
        if (_minBombs > _maxBombs)
        {
            _minBombs = _maxBombs;
        }
        _totalBombs = Random.Range(_minBombs, _maxBombs + 1);
    }

    void SpawnBombs()
    {
        HashSet<Vector2Int> bombPositions = new HashSet<Vector2Int>();

        while (bombPositions.Count < _totalBombs)
        {
            bombPositions.Add(new Vector2Int(Random.Range(0, 8), Random.Range(0, 8)));
        }

        foreach (Vector2Int pos in bombPositions)
        {
            Vector3 worldPos = new Vector3(pos.x * _tileSize - _boardCenter.x, 0, pos.y * _tileSize - _boardCenter.z);
            Instantiate(_bombPrefab, worldPos, Quaternion.identity, _bombs.transform);
        }
    }
}
