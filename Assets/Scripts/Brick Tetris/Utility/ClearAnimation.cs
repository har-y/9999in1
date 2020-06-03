﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAnimation : MonoBehaviour
{
    public Animator _animator;

    // Start is called before the first frame update
    void Start()
    {

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
