﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCar_Colider : MonoBehaviour
{
    public bool isCollided;
    public GameObject _brickCar;

    private Rigidbody2D _rb;
    private Animator _animator;
    

    // Start is called before the first frame update
    void Start()
    {
        isCollided = false;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isCollided = true;
            _rb.isKinematic = true;           
            Animation();
        }
    }

    private void Animation()
    {
        _brickCar.SetActive(false);
        _animator.SetTrigger("brick_death");
    }
}
