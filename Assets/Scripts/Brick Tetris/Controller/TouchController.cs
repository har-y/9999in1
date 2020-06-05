using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler SwipeEvent;

    public Text diagnosticText1;
    public Text diagnosticText2;

    public bool useDiagnostic = false;

    private Vector2 _touchMovement;

    private int _minimumSwipeDistance = 20;

    // Start is called before the first frame update
    void Start()
    {
        Diagnostic(" ", " ");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                _touchMovement = Vector2.zero;
                Diagnostic(" ", " ");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _touchMovement += touch.deltaPosition;
                if (_touchMovement.magnitude > _minimumSwipeDistance)
                {
                    OnSwipe();
                    Diagnostic(SwipeDiagnostic(_touchMovement), _touchMovement.ToString());
                }
            }
        }
    }

    private void OnSwipe()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(_touchMovement);
        }
    }

    private void Diagnostic(string text1, string text2)
    {
        diagnosticText1.gameObject.SetActive(useDiagnostic);
        diagnosticText2.gameObject.SetActive(useDiagnostic);

        if (diagnosticText1 && diagnosticText2)
        {
            diagnosticText1.text = text1;
            diagnosticText2.text = text2;
        }
    }

    private string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = " ";

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        }
        else
        {
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        }

        return direction;
    }
}
