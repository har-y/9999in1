﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTetris_ClearAnimation : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BrickRowClear(int y)
    {
        transform.position = new Vector3(5f, y, 0f);
        _animator.SetTrigger("row_clear");
    }
}
