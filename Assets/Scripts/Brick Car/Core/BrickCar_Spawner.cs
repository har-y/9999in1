using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_Spawner : MonoBehaviour
{
    public BrickCar_BrickShape playerShape;
    public BrickCar_BrickShape[] enemyShapes;

    private BrickCar_BrickShape enemyShape;

    private BrickCar_BrickShape _queuedPlayerShape;
    private BrickCar_BrickShape _queuedEnemyShape;

    public Transform playerPosition;
    public Transform enemyContrainer;

    // Start is called before the first frame update
    void Start()
    {
        InitQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
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


    private void InitQueue()
    {
        //_queuedPlayerShape = null;
        //_queuedEnemyShape = null;

        //FillQueue();
    }

    private void FillQueue()
    {
        //if (!_queuedPlayerShape)
        //{
        //    SpawnPlayerShape();
        //}

        //if (!_queuedEnemyShape)
        //{
        //    SpawnEnemyShape();
        //}
    }

    public BrickCar_BrickShape SpawnPlayerShape()
    {
        _queuedPlayerShape = Instantiate(GetPlayerShape(), playerPosition.position, Quaternion.identity);
        _queuedPlayerShape.transform.parent = playerPosition.transform;

        if (_queuedPlayerShape)
        {
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
        _queuedEnemyShape = Instantiate(GetRandomEnemyShape(), transform.position, Quaternion.identity);
        _queuedEnemyShape.transform.position = transform.position + _queuedEnemyShape._queuedOffset;
        _queuedEnemyShape.transform.parent = enemyContrainer.transform;

        if (_queuedEnemyShape)
        {
            return _queuedEnemyShape;
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }
}
