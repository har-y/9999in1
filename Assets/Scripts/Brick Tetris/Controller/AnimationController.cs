using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
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

    public void BrickRowClear()
    {
        transform.position = new Vector3(5f, 0f, 0f);
        _animator.SetTrigger("row_clear");
    }
}
