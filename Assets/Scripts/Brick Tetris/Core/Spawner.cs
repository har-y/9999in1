using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public BrickShape[] brickShapes;
    public Transform queuedForms;

    private BrickShape _queuedShape;

    private float _queueScale = 0.76f;

    // Start is called before the first frame update
    void Start()
    {
        InitQueue();
        FillQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private BrickShape GetRandomShape()
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

    public BrickShape SpawnShape()
    {
        BrickShape shape = null;

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
    }

    private void FillQueue()
    {
        if (!_queuedShape)
        {
            _queuedShape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as BrickShape;
            _queuedShape.transform.position = queuedForms.position + _queuedShape._queuedOffset;
            _queuedShape.transform.localScale = new Vector3(_queueScale, _queueScale, _queueScale);
        }
    }

    private BrickShape GetQueuedShape()
    {
        BrickShape firstShape = null;

        if (_queuedShape)
        {
            firstShape = _queuedShape;
        }

        _queuedShape = null;

        FillQueue();

        return firstShape;
    }
}
