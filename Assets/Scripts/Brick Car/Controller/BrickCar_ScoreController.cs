using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickCar_ScoreController : MonoBehaviour
{
    public Text _levelText;
    public Text _scoreText;
    public Text _highScoreText;

    public int _level = 0;
    public int carsPerLevel = 5;

    public bool isLevelUp;

    private string _highscoreKey = "HighScore";

    private int _score = 0;
    private int _highScore = 0;
    private int _cars;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt(_highscoreKey, 0);
        UpdateTextUI();
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelReset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScoreCars()
    {
        _score += 100 * _level;

        UpdateTextUI();
    }

    public void ScoreCars(int cars)
    {
        isLevelUp = false;        

        _cars -= cars;

        if (_cars <= 0)
        {
            LevelUp();
            UpdateTextUI();
        }

        UpdateTextUI();
    }

    public void LevelReset()
    {
        _level = 1;
        _cars = carsPerLevel * _level;

        UpdateTextUI();
    }

    private void UpdateTextUI()
    {
        if (_levelText)
        {
            _levelText.text = _level.ToString();
        }

        if (_scoreText)
        {
            _scoreText.text = PadZero(_score, 12);
        }

        if (_highScoreText)
        {
            if (_score > _highScore)
            {
                int tmpValue = _score;
                PlayerPrefs.SetInt(_highscoreKey, tmpValue);
                _highScoreText.text = PadZero(tmpValue, 12);
            }
            else
            {
                _highScoreText.text = PadZero(_highScore, 12);
            }
        }
    }

    private string PadZero(int n, int padDigits)
    {
        string nString = n.ToString();

        while (nString.Length < padDigits)
        {
            nString = "0" + nString;
        }

        return nString;
    }

    public void LevelUp()
    {
        _level++;
        _cars = carsPerLevel * _level;
        isLevelUp = true;
    }
}
