using System;
using System.Collections;
using System.Collections.Generic;
using Shared;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyManager Instance { get; private set; }

    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private List<EnemyDictionaryEntry> enemyList;

    [SerializeField] private bool testSpawnEnemy;

    private Vector3 playerPosition;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = playerTransform.position;
        if (testSpawnEnemy)
        {
            SpawnEnemy(enemyList[0].gameObject, new Vector3(0, 0,0 ), Quaternion.identity);
            testSpawnEnemy = false;
        }
    }

    public void SpawnEnemy(GameObject enemy, Vector3 position, Quaternion rotation)
    {
        GameObject newEnemy = Instantiate(enemy, position, rotation);
    }

    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }
}


[Serializable]
struct EnemyDictionaryEntry
{
    public EnemyType type;
    public GameObject gameObject;
}

enum EnemyType
{
    TestEnemy
}
