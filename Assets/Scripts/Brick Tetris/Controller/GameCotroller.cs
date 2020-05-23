using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCotroller : MonoBehaviour
{
    private BackgroundGrid _backgroundGrid;
    private Spawner _spawner;

    //info:: current shape
    private BrickShape _activeShape;

    private float _timeInterval = 1f;
    private float _time;
    private float _timeNextKey;

    [Range(0.02f, 1f)] public float _timeRepeatRateKey = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        _timeNextKey = Time.time;

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
        else
        {
            _spawner.transform.position = Vectorf.Round(_spawner.transform.position);

            if (!_activeShape)
            {
                _activeShape = _spawner.SpawnShape();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_backgroundGrid || _spawner || _activeShape)
        {
            return;
        }

        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > _timeNextKey || Input.GetButtonDown("MoveRight"))
        {
            _activeShape.MoveRight();
            _timeNextKey = Time.time + _timeRepeatRateKey;

            if (_backgroundGrid.IsValidPosition(_activeShape))
            {
                Debug.Log("move right");
            }
            else
            {
                _activeShape.MoveLeft();

                Debug.Log("hit the right boundary");
            }
        }

        if (!_backgroundGrid || !_spawner)
        {
            return;
        }

        if (Time.time > _time)
        {
            _time = Time.time + _timeInterval;

            if (_activeShape)
            {
                _activeShape.MoveDown();

                if (!_backgroundGrid.IsValidPosition(_activeShape))
                {
                    _activeShape.MoveUp();

                    _backgroundGrid.StoreShapeInGrid(_activeShape);

                    if (_spawner)
                    {
                        _activeShape = _spawner.SpawnShape();
                    }
                }
            }
        }
    }
}
