using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTetris_Spawner : MonoBehaviour
{
    public Transform queuedForms;
    public BrickTetris_BrickShape[] brickShapes;

    private BrickTetris_BrickShape _queuedShape;

    private float _queueScale = 0.76f;

    // Start is called before the first frame update
    void Start()
    {
        InitQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private BrickTetris_BrickShape GetRandomShape()
    {
        int i = Random.Range(0, brickShapes.Length);

        if (brickShapes[i])
        {
            return brickShapes[i];
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }

    public BrickTetris_BrickShape SpawnShape()
    {
        BrickTetris_BrickShape shape = null;

        shape = GetQueuedShape();

        shape.transform.position = transform.position;
        shape.transform.localScale = Vector3.one;

        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.Log("invalid shape");
            return null;
        }
    }

    private void InitQueue()
    {
        _queuedShape = null;

        FillQueue();
    }

    private void FillQueue()
    {
        if (!_queuedShape)
        {
            _queuedShape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);

            _queuedShape.transform.position = queuedForms.position + _queuedShape._queuedOffset;
            _queuedShape.transform.localScale = new Vector3(_queueScale, _queueScale, _queueScale);
        }
    }

    private BrickTetris_BrickShape GetQueuedShape()
    {
        BrickTetris_BrickShape firstShape = null;

        if (_queuedShape)
        {
            firstShape = _queuedShape;
        }

        _queuedShape = null;

        FillQueue();

        return firstShape;
    }
}
