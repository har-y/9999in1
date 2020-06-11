﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_BackgroundGrid : MonoBehaviour
{
    public Transform brick;

    public int height = 35;
    public int width = 11;
    public int header = 8;

    private Transform[ , ] _grid;

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
            for (int y = 0; y < height - header; y++)
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

    private bool IsInGrid(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    private bool IsValidOccupied(int x, int y, BrickTetris_BrickShape shape)
    {
        return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(BrickTetris_BrickShape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 position = BrickTetris_Vectorf.Round(child.position);

            if (!IsInGrid((int)position.x, (int)position.y))
            {
                return false;
            }

            if (IsValidOccupied((int)position.x, (int)position.y, shape))
            {
                return false;
            }
        }
        return true;
    }

    public void StoreShapeInGrid(BrickTetris_BrickShape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 position = BrickTetris_Vectorf.Round(child.position);
            _grid[(int)position.x, (int)position.y] = child;
        }
    }

    public bool IsOverLimit(BrickTetris_BrickShape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (height - header))
            {
                return true;
            }
        }
        return false;
    }
}