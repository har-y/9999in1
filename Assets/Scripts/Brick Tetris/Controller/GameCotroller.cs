using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCotroller : MonoBehaviour
{
    private BackgroundGrid _backgroundGrid;
    private Spawner _spawner;

    //info:: current shape
    private BrickShape _activeShape;

    private float _interval = 1f;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        _backgroundGrid = FindObjectOfType<BackgroundGrid>();
        if (!_backgroundGrid)
        {
            Debug.Log("not assign object");
        }

        _spawner = FindObjectOfType<Spawner>();
        if (!_spawner)
        {
            Debug.Log("not assign object");
        }

        if (_spawner)
        {
            if (_activeShape == null)
            {
                _activeShape = _spawner.SpawnShape();
            }
            _spawner.transform.position = Vectorf.Round(_spawner.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_backgroundGrid || !_spawner)
        {
            return;
        }

        if (Time.time > _time)
        {
            _time = Time.time + _interval;

            if (_activeShape)
            {
                _activeShape.MoveDown();
            }
        }
    }
}
