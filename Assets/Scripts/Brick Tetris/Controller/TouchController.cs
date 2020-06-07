using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler DragEvent;
    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler TapEvent;

    public bool useDiagnostic = false;

    [Range(50, 150)] public int _minimumDragDistance = 100;
    [Range(50, 250)] public int _minimumSwipeDistance = 200;

    public Text diagnosticText1;
    public Text diagnosticText2;

    private Vector2 _touchMovement;

    private float _tapTimeMax = 0;
    public float _tapTimeWindow = 0.1f;

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
                _tapTimeMax = Time.time + _tapTimeWindow;

                Diagnostic(" ", " ");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _touchMovement += touch.deltaPosition;

                if (_touchMovement.magnitude > _minimumDragDistance)
                {
                    OnDrag();
                    Diagnostic("drag: " + SwipeDiagnostic(_touchMovement), _touchMovement.ToString());
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_touchMovement.magnitude > _minimumSwipeDistance)
                {
                    OnSwipeEnd();
                    Diagnostic("swipe: " + SwipeDiagnostic(_touchMovement), _touchMovement.ToString());
                }
                else if (Time.time < _tapTimeMax)
                {
                    OnTap();
                    Diagnostic("tap: " + SwipeDiagnostic(_touchMovement), _touchMovement.ToString());
                }
            }
        }
    }

    private void OnDrag()
    {
        if (DragEvent != null)
        {
            DragEvent(_touchMovement);
        }
    }

    private void OnSwipeEnd()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(_touchMovement);
        }
    }

    private void OnTap()
    {
        if (TapEvent != null)
        {
            TapEvent(_touchMovement);
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
