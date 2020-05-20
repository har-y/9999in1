using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickShape : MonoBehaviour
{
    public bool canRotate = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BrickMove(Vector3 direction)
    {
        transform.position += direction;
    }

    public void MoveLeft()
    {
        BrickMove(new Vector3(-1f, 0f, 0f));
    }

    public void MoveRight()
    {
        BrickMove(new Vector3(1f, 0f, 0f));
    }

    public void MoveDown()
    {
        BrickMove(new Vector3(0f, -1f, 0f));
    }

    public void MoveUp()
    {
        BrickMove(new Vector3(0f, 1f, 0f));
    }

    public void RotateLeft()
    {
        if (canRotate)
        {
            transform.Rotate(0f, 0f, 90f);
        }
    }

    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0f, 0f, -90f);
        }
    }
}
