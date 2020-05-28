using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int _score = 0;
    private int _level = 0;
    private int _lines;

    public int linesPerLevel = 5;

    private const int _minLines = 1;
    private const int _maxLines = 4;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreLines(int lines)
    {
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
    }

    public void Reset()
    {
        _level = 1;
        _lines = linesPerLevel * _level;
    }
}
