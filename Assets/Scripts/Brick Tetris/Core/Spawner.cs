using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public BrickShape[] brickShapes;

    // Start is called before the first frame update
    void Start()
    {
        
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

    private BrickShape SpawnShape()
    {
        BrickShape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as BrickShape;

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
}
