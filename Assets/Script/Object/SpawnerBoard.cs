using System;
using UnityEngine;

public class SpawnerBoard : MonoBehaviour
{
    [SerializeField] private GameObject _boardPrefab;

    private void Start()
    {
        Instantiate(_boardPrefab, transform);
    }
}
