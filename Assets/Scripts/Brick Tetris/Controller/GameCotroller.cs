﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCotroller : MonoBehaviour
{
    public  float _timeInterval = 0.9f;

    private BackgroundGrid _backgroundGrid;
    private BrickShape _activeShape;
    private Spawner _spawner;
    private AudioManager _audioManager;

    private float _timeNextDown;

    private bool _isGameOver;

    [Range(0.02f, 1f)] public float _timeRepeatRateLeftKey = 0.10f;
    private float _timeNextLeftKey;

    [Range(0.02f, 1f)] public float _timeRepeatRateRightKey = 0.10f;
    private float _timeNextRightKey;

    [Range(0.02f, 1f)] public float _timeRepeatRateRotateKey = 0.25f;
    private float _timeNextRotateKey;

    [Range(0.01f, 1f)] public float _timeRepeatRateDownKey = 0.01f;
    private float _timeNextDownKey;

    [SerializeField] private GameObject _pausePanel;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeNextLeftKey = Time.time + _timeRepeatRateLeftKey;
        _timeNextRightKey = Time.time + _timeRepeatRateRightKey;
        _timeNextDownKey = Time.time + _timeRepeatRateDownKey;
        _timeNextDownKey = Time.time + _timeRepeatRateRotateKey;

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

        _audioManager = FindObjectOfType<AudioManager>();
        if (!_audioManager)
        {
            Debug.Log("not assign object");
        }  
    }

    // Update is called once per frame
    void Update()
    {
        if (!_backgroundGrid || !_spawner || !_activeShape || !_audioManager || _isGameOver)
        {
            return;
        }

        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > _timeNextLeftKey || Input.GetButtonDown("MoveRight"))
        {
            _activeShape.MoveRight();
            _timeNextLeftKey = Time.time + _timeRepeatRateRightKey;


            if (!_backgroundGrid.IsValidPosition(_activeShape))
            {
                _activeShape.MoveLeft();
                PlaySound(_audioManager.errorSound, 0.35f);
            }
            else
            {
                PlaySound(_audioManager.moveSound, 0.25f);
            }
        }
        else if (Input.GetButton("MoveLeft") && Time.time > _timeNextRightKey || Input.GetButtonDown("MoveLeft"))
        {
            _activeShape.MoveLeft();
            _timeNextRightKey = Time.time + _timeRepeatRateLeftKey;

            if (!_backgroundGrid.IsValidPosition(_activeShape))
            {
                _activeShape.MoveRight();
                PlaySound(_audioManager.errorSound, 0.35f);
            }
            else
            {
                PlaySound(_audioManager.moveSound, 0.25f);
            }
        }
        else if (Input.GetButtonDown("Rotate") && Time.time > _timeNextRotateKey)
        {
            _activeShape.RotateRight();
            _timeNextRotateKey = Time.time + _timeRepeatRateRotateKey;

            if (!_backgroundGrid.IsValidPosition(_activeShape))
            {
                _activeShape.RotateLeft();
                PlaySound(_audioManager.errorSound, 0.35f);
            }
            else
            {
                PlaySound(_audioManager.moveSound, 0.25f);
            }
        }
        else if (Input.GetButton("MoveDown") && (Time.time > _timeNextDownKey) || (Time.time > _timeNextDown))
        {
            _timeNextDown = Time.time + _timeInterval;
            _timeNextDownKey = Time.time + _timeRepeatRateDownKey;

            _activeShape.MoveDown();

            if (!_backgroundGrid.IsValidPosition(_activeShape))
            {
                if (_backgroundGrid.IsOverLimit(_activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }
        }
    }


    private void LandShape()
    {
        _activeShape.MoveUp();
        _backgroundGrid.StoreShapeInGrid(_activeShape);

        PlaySound(_audioManager.dropSound, 0.75f);

        _activeShape = _spawner.SpawnShape();

        _timeNextLeftKey = Time.time;
        _timeNextRightKey = Time.time;
        _timeNextDownKey = Time.time;
        _timeNextDownKey = Time.time;

        _backgroundGrid.ClearAllRows();

        if (_backgroundGrid.completedRows > 0)
        {
            PlaySound(_audioManager.clearRowSound, 0.25f);
        }
    }

    private void GameOver()
    {
        _activeShape.MoveUp();

        PlaySound(_audioManager.loseSound, 1f);

        _isGameOver = true;
        Debug.Log(_activeShape.name + " is over the limit");
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void Play()
    {
        Debug.Log("Play");
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }

    public void Pause()
    {
        Debug.Log("Paused");
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (_audioManager.fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(_audioManager.fxVolume * volume, 0.05f, 1f));
        }
    }
}
