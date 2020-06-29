using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMenu_Option : MonoBehaviour
{
    public Sprite[] _sprite;

    private SpriteRenderer _renderer;
    private BrickMenu_GameController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = FindObjectOfType<BrickMenu_GameController>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SpriteOptionChange();
    }

    private void SpriteOptionChange()
    {
        int option = _controller._option;

        switch (option)
        {
            case 0:
                _renderer.sprite = _sprite[0];
                transform.position = new Vector3(5f, 13.5f, 0f);
                break;
            case 1:
                _renderer.sprite = _sprite[1];
                transform.position = new Vector3(5.5f, 13.5f, 0f);
                break;
            default:
                break;
        }
    }
}
