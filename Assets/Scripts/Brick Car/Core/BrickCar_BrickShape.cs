using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_BrickShape : MonoBehaviour
{
    public Vector3 _queuedOffset;

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
}
