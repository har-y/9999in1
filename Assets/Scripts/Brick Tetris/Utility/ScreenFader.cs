using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public float startAlpha = 1f;
    public float targetAlpha = 0f;

    private float currentAlpha;
    private float _increment;

    public float delay = 0f;
    public float timeToFade = 1f;

    private MaskableGraphic _graphic;
    private Color originalColor;

    // Use this for initialization
    void Start()
    {
        _graphic = GetComponent<MaskableGraphic>();

        originalColor = _graphic.color;

        currentAlpha = startAlpha;

        Color tempColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
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

        while (Mathf.Abs(targetAlpha - currentAlpha) > 0.01f)
        {
            yield return new WaitForEndOfFrame();

            currentAlpha = currentAlpha + _increment;

            Color tempColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
            _graphic.color = tempColor;
        }

        Debug.Log("screen fader end");
    }
}
