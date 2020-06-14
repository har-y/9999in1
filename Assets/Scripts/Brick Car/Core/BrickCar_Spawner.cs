using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_Spawner : MonoBehaviour
{
    public BrickCar_BrickShape playerShape;
    public BrickCar_BrickShape[] enemyShapes;

    public Transform playerPosition;
    public Transform enemyContrainer;

    private BrickCar_BrickShape _queuedPlayerShape;
    public BrickCar_BrickShape _queuedEnemyShape;

    public float enemySpawnTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private BrickCar_BrickShape GetPlayerShape()
    {
        if (playerShape)
        {
            return playerShape;
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }

    private BrickCar_BrickShape GetRandomEnemyShape()
    {
        int i = Random.Range(0, enemyShapes.Length);

        if (enemyShapes[i])
        {
            return enemyShapes[i];
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }

    public BrickCar_BrickShape SpawnPlayerShape()
    {
        if (!_queuedPlayerShape)
        {
            _queuedPlayerShape = Instantiate(GetPlayerShape(), playerPosition.position, Quaternion.identity);
            _queuedPlayerShape.transform.parent = playerPosition.transform;
            return _queuedPlayerShape;
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }

    public BrickCar_BrickShape SpawnEnemyShape()
    {
        if (!_queuedEnemyShape)
        {
            _queuedEnemyShape = Instantiate(GetRandomEnemyShape(), transform.position, Quaternion.identity);
            _queuedEnemyShape.transform.position = transform.position + _queuedEnemyShape._queuedOffset;
            _queuedEnemyShape.transform.parent = enemyContrainer.transform;
            return _queuedEnemyShape;
        }
        else if (_queuedEnemyShape)
        {
            _queuedEnemyShape = Instantiate(GetRandomEnemyShape(), transform.position, Quaternion.identity);
            _queuedEnemyShape.transform.position = transform.position + _queuedEnemyShape._queuedOffset;
            _queuedEnemyShape.transform.parent = enemyContrainer.transform;

            return _queuedEnemyShape;
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }
}
