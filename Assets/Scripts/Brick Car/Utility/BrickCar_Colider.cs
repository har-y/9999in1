using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_Colider : MonoBehaviour
{
    public bool isCollided;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        isCollided = false;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            isCollided = true;
            _rb.isKinematic = true;
        }
    }
}
