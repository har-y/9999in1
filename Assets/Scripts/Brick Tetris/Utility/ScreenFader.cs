using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public float startAlpha = 1f;
    public float targetAlpha = 0f;
    public float delay = 0f;
    public float timeToFade = 1f;

    private MaskableGraphic _graphic;
    private Color _originalColor;

    private float _currentAlpha;
    private float _increment;

    // Use this for initialization
    void Start()
    {
        _graphic = GetComponent<MaskableGraphic>();

        _originalColor = _graphic.color;

        _currentAlpha = startAlpha;

        Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
        _graphic.color = tempColor;

        _increment = ((targetAlpha - startAlpha) / timeToFade) * Time.deltaTime;

        StartCoroutine(FadeRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(delay);

        while (Mathf.Abs(targetAlpha - _currentAlpha) > 0.01f)
        {
            yield return new WaitForEndOfFrame();

            _currentAlpha = _currentAlpha + _increment;

            Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
            _graphic.color = tempColor;
        }

        Debug.Log("screen fader end");
    }
}
