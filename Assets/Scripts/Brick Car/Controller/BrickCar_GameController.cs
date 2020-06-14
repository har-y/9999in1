﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickCar_GameController : MonoBehaviour
{
    [Range(0.02f, 1f)] public float _timeRepeatRateLeftKey = 0.30f;
    [Range(0.02f, 1f)] public float _timeRepeatRateRightKey = 0.30f;
    [Range(0.01f, 1f)] public float _timeRepeatRateDownKey = 0.01f;

    [Range(0.05f, 1f)] public float _timeRepeatSwipe = 0.20f;
    [Range(0.05f, 1f)] public float _timeRepeatDrag = 0.20f;

    [SerializeField] private GameObject _pausePanel;

    public bool _isPaused = false;

    public float _timeInterval = 0.9f;
    public float _timeEnemyInterval = 0.9f;
    public float _timeEnemySpawnInterval = 3f;

    private Animator _animator;
    private BrickCar_BackgroundGrid _backgroundGrid;
    private BrickCar_BrickShape _playerShape;
    private BrickCar_BrickShape _enemyShape;
    private BrickCar_Spawner _spawner;
    private BrickCar_AudioManager _audioManager;
    private BrickCar_ScoreController _scoreController;

    //private GameObject[] _enemyShapes;


    private enum Direction { none, left, right, up, down }

    private Direction _swipeDirection = Direction.none;
    private Direction _dragDirection = Direction.none;

    private float _timeNextLeftKey;
    private float _timeNextRightKey;
    private float _timeNextRotateKey;
    private float _timeNextDownKey;

    private float _dropTimeInterval;
    private float _dropTimeEnemyInterval;
    private float _dropTimeEnemySpawnInterval;
    private float _timeNextDown;
    private float _timeNextEnemyDown;
    private float _timeNextEnemySpawn;

    private float _timeNextSwipe;
    private float _timeNextDrag;

    private bool _isGameOver = false;
    private bool _didTap = false;

    // Start is called before the first frame update
    void Start()
    {
        _timeNextLeftKey = Time.time + _timeRepeatRateLeftKey;
        _timeNextRightKey = Time.time + _timeRepeatRateRightKey;
        _timeNextDownKey = Time.time + _timeRepeatRateDownKey;

        _dropTimeInterval = _timeInterval;
        _dropTimeEnemyInterval = _timeEnemyInterval;
        _dropTimeEnemySpawnInterval = _timeEnemySpawnInterval;

        _backgroundGrid = FindObjectOfType<BrickCar_BackgroundGrid>();
        if (!_backgroundGrid)
        {
            Debug.Log("not assign object");
        }

        _spawner = FindObjectOfType<BrickCar_Spawner>();
        if (!_spawner)
        {
            Debug.Log("not assign object");
        }
        else
        {
            _spawner.transform.position = BrickCar_Vectorf.Round(_spawner.transform.position);

            if (!_playerShape)
            {
                _playerShape = _spawner.SpawnPlayerShape();              
            }
        }

        //_audioManager = FindObjectOfType<BrickCar_AudioManager>();
        //if (!_audioManager)
        //{
        //    Debug.Log("not assign object");
        //}

        //_scoreController = FindObjectOfType<BrickCar_ScoreController>();
        //if (!_scoreController)
        //{
        //    Debug.Log("not assign object");
        //}

        //_animator = GetComponent<Animator>();
        //if (!_animator)
        //{
        //    Debug.Log("not assign object");
        //}

        if (_pausePanel)
        {
            _pausePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_backgroundGrid || !_spawner || !_playerShape )// || !_audioManager || !_scoreController || _isGameOver)
        {
            return;
        }

        PlayerInput();
        Enemy();
    }

    private void OnEnable()
    {
        BrickTetris_TouchController.SwipeEvent += SwipeHandler;
        BrickTetris_TouchController.DragEvent += DragHandler;
        BrickTetris_TouchController.TapEvent += TapHandler;
    }

    private void OnDisable()
    {
        BrickTetris_TouchController.SwipeEvent -= SwipeHandler;
        BrickTetris_TouchController.DragEvent -= DragHandler;
        BrickTetris_TouchController.TapEvent -= TapHandler;
    }

    private void PlayerInput()
    {
        if ((Input.GetButton("MoveRight") && Time.time > _timeNextRightKey) || Input.GetButtonDown("MoveRight"))
        {
            MoveRight();
        }
        else if ((Input.GetButton("MoveLeft") && Time.time > _timeNextLeftKey) || Input.GetButtonDown("MoveLeft"))
        {
            MoveLeft();
        }
        else if ((Input.GetButton("MoveDown") && (Time.time > _timeNextDownKey)) || (Time.time > _timeNextDown))
        {
            MoveDown();           
        }
        else if ((_swipeDirection == Direction.right && Time.time > _timeNextSwipe) || (_dragDirection == Direction.right && Time.time > _timeNextDrag))
        {
            MoveRight();

            _timeNextSwipe = Time.time + _timeRepeatSwipe;
            _timeNextDrag = Time.time + _timeRepeatDrag;
        }
        else if ((_swipeDirection == Direction.left && Time.time > _timeNextSwipe) || (_dragDirection == Direction.left && Time.time > _timeNextDrag))
        {
            MoveLeft();

            _timeNextSwipe = Time.time + _timeRepeatSwipe;
            _timeNextDrag = Time.time + _timeRepeatDrag;
        }
        else if ((_swipeDirection == Direction.up && Time.time > _timeNextSwipe) || (_didTap))
        {
            MoveDown();

            _timeNextSwipe = Time.time + _timeRepeatSwipe;
            _didTap = false;
        }
        else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }

        _dragDirection = Direction.none;
        _swipeDirection = Direction.none;
        _didTap = false;
    }

    private void Enemy()
    {
        if (!_isGameOver)
        {
            if (Time.time > _timeNextEnemyDown)
            {
                EnemyMoveDown();
            }

            if (Time.time > _timeNextEnemySpawn)
            {
                EnemySpawn();
            }

            EnemyDestroy();
        }
    }

    private void EnemyDestroy()
    {
        GameObject[] enemyShapes = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject shape in enemyShapes)
        {
            if (_backgroundGrid.IsBelowLimit(shape.GetComponent<BrickCar_BrickShape>()))
            {
                Destroy(shape);
            }
        }
    }

    private void EnemyMoveDown()
    {
        _timeNextEnemyDown = Time.time + _dropTimeEnemyInterval;

        GameObject[] enemyshapes = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject shape in enemyshapes)
        {
            shape.GetComponent<BrickCar_BrickShape>().MoveDown();
        }
    }

    private void EnemySpawn()
    {
        _timeNextEnemySpawn = Time.time + _dropTimeEnemySpawnInterval;
        _enemyShape = _spawner.SpawnEnemyShape();
    }

    private void MoveDown()
    {
        _timeNextDown = Time.time + _dropTimeInterval;
        _timeNextDownKey = Time.time + _timeRepeatRateDownKey;
    }

    private void MoveLeft()
    {
        _playerShape.MoveLeft();
        _timeNextLeftKey = Time.time + _timeRepeatRateLeftKey;

        if (!_backgroundGrid.IsValidPosition(_playerShape))
        {
            _playerShape.MoveRight();
            //PlaySound(_audioManager.errorSound, 0.35f);
        }
        //else
        //{
        //    PlaySound(_audioManager.moveSound, 0.25f);
        //}
    }

    private void MoveRight()
    {
        _playerShape.MoveRight();
        _timeNextRightKey = Time.time + _timeRepeatRateRightKey;

        if (!_backgroundGrid.IsValidPosition(_playerShape))
        {
            _playerShape.MoveLeft();
            //PlaySound(_audioManager.errorSound, 0.35f);
        }
        //else
        //{
        //    PlaySound(_audioManager.moveSound, 0.25f);
        //}
    }

    private void LandShape()
    {

        //PlaySound(_audioManager.dropSound, 0.75f);

        _timeNextLeftKey = Time.time;
        _timeNextRightKey = Time.time;
        _timeNextDownKey = Time.time;
        _timeNextDownKey = Time.time;

        //_backgroundGrid.StartCoroutine("ClearAllRows");

        //if (_backgroundGrid.completedRows > 0)
        //{
        //    _scoreController.ScoreLines(_backgroundGrid.completedRows);

        //    if (_scoreController.isLevelUp)
        //    {
        //        PlaySound(_audioManager.levelUpSound, 0.35f);
        //        _dropTimeInterval = Mathf.Clamp(_timeInterval - (((float)_scoreController._level - 1) * 0.05f), 0.05f, 1f);
        //    }
        //    else
        //    {
        //        PlaySound(_audioManager.clearRowSound, 0.25f);
        //    }

        //    //PlaySound(_audioManager.clearRowSound, 0.15f);
        //}
    }

    private void SwipeHandler(Vector2 swipeMovement)
    {
        _swipeDirection = GetDirection(swipeMovement);
    }

    private void DragHandler(Vector2 dragMovement)
    {
        _dragDirection = GetDirection(dragMovement);
    }

    private void TapHandler(Vector2 tapMovement)
    {
        _didTap = true;
    }

    Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDirection = Direction.none;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDirection = (swipeMovement.x >= 0) ? Direction.right : Direction.left;
        }
        else
        {
            swipeDirection = (swipeMovement.y >= 0) ? Direction.up : Direction.down;
        }

        return swipeDirection;
    }

    private void GameOver()
    {
        _isGameOver = true;

        _playerShape.MoveUp();

        PlaySound(_audioManager.loseSound, 1f);

        Debug.Log(_playerShape.name + " is over the limit");

        PlaySound(_audioManager.gameOverSound, 0.25f);

        _animator.SetTrigger("brick_gameover");
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Debug.Log("Restarted");
        SceneManager.LoadScene(0);
    }

    public void RestartClear()
    {
        if (_isGameOver)
        {
            BrickCar_BrickShape[] shapes = FindObjectsOfType<BrickCar_BrickShape>();

            for (int i = 0; i < shapes.Length; i++)
            {
                Destroy(shapes[i].gameObject);
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (_audioManager.fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(_audioManager.fxVolume * volume, 0.05f, 1f));
        }
    }

    public void TogglePause()
    {
        if (_isGameOver)
        {
            return;
        }

        _isPaused = !_isPaused;

        if (_pausePanel)
        {
            _pausePanel.SetActive(_isPaused);

            if (_audioManager)
            {
                _audioManager.musicSource.volume = (_isPaused) ? _audioManager.musicVolume * 0.25f : _audioManager.musicVolume;
            }

            Time.timeScale = (_isPaused) ? 0 : 1;
        }
    }
}
