using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_Edge : MonoBehaviour
{
    public Transform point;
    public Transform otherBlock;

    private float _halfLenght = 32f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EdgeMoveEndless();
    }

    private void EdgeMoveEndless()
    {
        if (transform.position.y + _halfLenght < point.position.y)
        {
            transform.position = new Vector3(otherBlock.position.x, otherBlock.position.y + _halfLenght, otherBlock.position.z);
        }
    }
}
