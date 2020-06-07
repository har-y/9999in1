using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Range(0.02f, 1f)] public float _timeRepeatRateLeftKey = 0.10f;
    [Range(0.02f, 1f)] public float _timeRepeatRateRightKey = 0.10f;
    [Range(0.02f, 1f)] public float _timeRepeatRateRotateKey = 0.25f;
    [Range(0.01f, 1f)] public float _timeRepeatRateDownKey = 0.01f;

    [Range(0.05f, 1f)] public float _timeRepeatSwipe = 0.20f;
    [Range(0.05f, 1f)] public float _timeRepeatDrag = 0.20f;

    [SerializeField] private GameObject _pausePanel;

    public bool _isPaused = false;

    public  float _timeInterval = 0.9f;

    private BackgroundGrid _backgroundGrid;
    private BrickShape _activeShape;
    private Spawner _spawner;
    private AudioManager _audioManager;
    private ScoreController _scoreController;

    private enum Direction { none, left, right, up, down }

    private Direction _swipeDirection = Direction.none;
    private Direction _dragDirection = Direction.none;

    private float _timeNextLeftKey;
    private float _timeNextRightKey;
    private float _timeNextRotateKey;
    private float _timeNextDownKey;

    private float _dropTimeInterval;
    private float _timeNextDown;

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
        _timeNextRotateKey = Time.time + _timeRepeatRateRotateKey;

        _dropTimeInterval = _timeInterval;

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

        _scoreController = FindObjectOfType<ScoreController>();
        if (!_scoreController)
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
        if (!_backgroundGrid || !_spawner || !_activeShape || !_audioManager || !_scoreController || _isGameOver)
        {
            return;
        }

        PlayerInput();
    }

    private void OnEnable()
    {
        TouchController.SwipeEvent += SwipeHandler;
        TouchController.DragEvent += DragHandler;
        TouchController.TapEvent += TapHandler;
    }

    private void OnDisable()
    {
        TouchController.SwipeEvent -= SwipeHandler;
        TouchController.DragEvent -= DragHandler;
        TouchController.TapEvent -= TapHandler;
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
        else if (Input.GetButtonDown("Rotate") && Time.time > _timeNextRotateKey)
        {
            Rotate();
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
            Rotate();

            _timeNextSwipe = Time.time + _timeRepeatSwipe;
            _didTap = false;
        }
        else if (_dragDirection == Direction.down && Time.time > _timeNextDrag)
        {
            MoveDown();          
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

    private void Rotate()
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

    private void MoveLeft()
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

    private void MoveRight()
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

        _backgroundGrid.StartCoroutine("ClearAllRows");

        if (_backgroundGrid.completedRows > 0)
        {
            _scoreController.ScoreLines(_backgroundGrid.completedRows);

            if (_scoreController.isLevelUp)
            {
                PlaySound(_audioManager.levelUpSound, 0.35f);
                _dropTimeInterval = Mathf.Clamp(_timeInterval - (((float)_scoreController._level - 1) * 0.05f), 0.05f, 1f);
            }
            else
            {
                PlaySound(_audioManager.clearRowSound, 0.25f);
            }

            //PlaySound(_audioManager.clearRowSound, 0.15f);
        }
    }

    private void GameOver()
    {
        _activeShape.MoveUp();

        PlaySound(_audioManager.loseSound, 1f);

        _isGameOver = true;
        Debug.Log(_activeShape.name + " is over the limit");

        PlaySound(_audioManager.gameOverSound, 0.25f);
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
}
