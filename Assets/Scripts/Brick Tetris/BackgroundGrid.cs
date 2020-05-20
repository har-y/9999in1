using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGrid : MonoBehaviour
{
    public Transform brick;
    private Transform[ , ] _grid;

    public int height = 27;
    public int width = 11;


    private void Awake()
    {
        _grid = new Transform[width, height];
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DrawEmptyCells()
    {
        if (brick != null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone;
                    clone = Instantiate(brick, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = "Grid Space ( x = " + x.ToString() + " , y =" + y.ToString() + " )";
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("assign sprite object!");

        }

    }
}
