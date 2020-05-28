using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private int _score = 0;
    private int _level = 0;
    private int _lines;

    public int linesPerLevel = 5;

    public bool isLevelUp;

    private const int _minLines = 1;
    private const int _maxLines = 4;

    public Text _levelText;
    public Text _scoreText;
    public Text _highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        LevelReset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ScoreLines(int lines)
    {
        isLevelUp = false;

        lines = Mathf.Clamp(lines, _minLines, _maxLines);

        switch (lines)
        {
            case 1:
                _score += 50 * _level;
                break;
            case 2:
                _score += 100 * _level;
                break;
            case 3:
                _score += 300 * _level;
                break;
            case 4:
                _score += 400 * _level;
                break;
        }
        _lines -= lines;

        if (_lines <= 0)
        {
            LevelUp();
            UpdateTextUI();
        }

        UpdateTextUI();
    }

    public void LevelReset()
    {
        _level = 1;
        _lines = linesPerLevel * _level;

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
        _lines = linesPerLevel * _level;
        isLevelUp = true;
    }
}
