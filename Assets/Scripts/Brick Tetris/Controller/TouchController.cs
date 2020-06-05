using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler SwipeEvent;

    private Vector2 _touchMovement;

    private int _minimumSwipeDistance = 20;

    // Start is called before the first frame update
    void Start()
    {
        
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
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _touchMovement += touch.deltaPosition;
                if (_touchMovement.magnitude > _minimumSwipeDistance)
                {
                    OnSwipe();
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
}
