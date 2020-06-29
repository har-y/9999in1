using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickMenu_GameController : MonoBehaviour
{
    [Range(0.02f, 1f)] public float _timeRepeatRateLeftKey = 0.30f;
    [Range(0.02f, 1f)] public float _timeRepeatRateRightKey = 0.30f;
    [Range(0.01f, 10f)] public float _timeRepeatRateDownKey = 10f;

    [Range(0.05f, 1f)] public float _timeRepeatSwipe = 0.20f;
    [Range(0.05f, 1f)] public float _timeRepeatDrag = 0.20f;

    [SerializeField] private GameObject _pausePanel;

    public bool _isPaused = false;

    public int _option;

    public float _timeInterval = 0.9f;
    public float _timeEnemyInterval = 0.9f;
    public float _timeEnemySpawnInterval = 10f;
    public float _timeEnemyFastSpawnInterval = 2f;
    public float _timeEdgeInterval = 1.4f;
    public float _timeScoreInterval = 1f;

    private Animator _animator;
    private BrickMenu_BackgroundGrid _backgroundGrid;
    private BrickMenu_AudioManager _audioManager;

    private enum Direction { none, left, right, up, down }

    private Direction _swipeDirection = Direction.none;
    private Direction _dragDirection = Direction.none;


    private float _timeNextLeftKey;
    private float _timeNextRightKey;
    private float _timeNextRotateKey;
    private float _timeNextDownKey;

    private float _dropTimeInterval;
    private float _dropTimeEdgeInterval;
    private float _dropTimeEnemyInterval;
    private float _dropTimeEnemySpawnInterval;
    private float _dropTimeEnemyFastSpawnInterval;
    private float _dropTimeScoreInterval;
    private float _timeNextDown;
    private float _timeNextEdgeDown;
    private float _timeNextEnemyDown;
    private float _timeNextEnemySpawn;
    private float _timeNextEnemyFastSpawn;
    private float _timeNextScoreSpawn;

    private float _timeNextSwipe;
    private float _timeNextDrag;

    private bool _didTap = false;

    // Start is called before the first frame update
    void Start()
    {
        _timeNextLeftKey = Time.time + _timeRepeatRateLeftKey;
        _timeNextRightKey = Time.time + _timeRepeatRateRightKey;
        _timeNextDownKey = Time.time + _timeRepeatRateDownKey;

        _dropTimeInterval = _timeInterval;
        _dropTimeEdgeInterval = _timeEdgeInterval;
        _dropTimeEnemyInterval = _timeEnemyInterval;
        _dropTimeEnemySpawnInterval = _timeEnemySpawnInterval;
        _dropTimeEnemyFastSpawnInterval = _timeEnemyFastSpawnInterval;
        _dropTimeScoreInterval = _timeScoreInterval;

        _backgroundGrid = FindObjectOfType<BrickMenu_BackgroundGrid>();
        if (!_backgroundGrid)
        {
            Debug.Log("not assign object");
        }
      
        _audioManager = FindObjectOfType<BrickMenu_AudioManager>();
        if (!_audioManager)
        {
            Debug.Log("not assign object");
        }
        
        _animator = GetComponent<Animator>();
        if (!_animator)
        {
            Debug.Log("not assign object");
        }

        if (_pausePanel)
        {
            _pausePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_backgroundGrid || !_audioManager)
        {
            return;
        }

        PlayerInput();
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
            _option = 1;
        }
        else if ((Input.GetButton("MoveLeft") && Time.time > _timeNextLeftKey) || Input.GetButtonDown("MoveLeft"))
        {
            MoveLeft();
            _option = 0;
        }
        else if (Input.GetButton("MoveDown") && (Time.time > _timeNextDownKey) || Input.GetButton("MoveDown"))
        {
            _animator.Play("menu_change");
        }
        else if ((_swipeDirection == Direction.right && Time.time > _timeNextSwipe) || (_dragDirection == Direction.right && Time.time > _timeNextDrag))
        {
            MoveRight();

            _option = 1;

            _timeNextSwipe = Time.time + _timeRepeatSwipe;
            _timeNextDrag = Time.time + _timeRepeatDrag;
        }
        else if ((_swipeDirection == Direction.left && Time.time > _timeNextSwipe) || (_dragDirection == Direction.left && Time.time > _timeNextDrag))
        {
            MoveLeft();

            _option = 0;

            _timeNextSwipe = Time.time + _timeRepeatSwipe;
            _timeNextDrag = Time.time + _timeRepeatDrag;
        }
        else if ((_swipeDirection == Direction.up && Time.time > _timeNextSwipe) || (_didTap))
        {
            MoveDown();

            _animator.Play("menu_change");

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

    private void MoveDown()
    {
        _timeNextDown = Time.time + _dropTimeInterval;
        _timeNextDownKey = Time.time + _timeRepeatRateDownKey;
    }

    private void MoveLeft()
    {
        _timeNextLeftKey = Time.time + _timeRepeatRateLeftKey;
    }

    private void MoveRight()
    {
        _timeNextRightKey = Time.time + _timeRepeatRateRightKey;
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

    private void SceneSpriteClear()
    {
        BrickMenu_Option sprite = FindObjectOfType<BrickMenu_Option>();
        sprite.gameObject.SetActive(false);
    }

    private void SceneChange()
    {
        switch (_option)
        {
            case 0:
                SceneManager.LoadScene(1);
                break;
            case 1:
                SceneManager.LoadScene(2);
                break;
            default:
                break;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Debug.Log("Restarted");
        SceneManager.LoadScene(0);
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
