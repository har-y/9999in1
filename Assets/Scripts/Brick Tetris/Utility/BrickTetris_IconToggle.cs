using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BrickTetris_IconToggle : MonoBehaviour
{
    public Sprite icon_true;
    public Sprite icon_false;

    public bool defaultSoundState = true;

    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = (defaultSoundState) ? icon_true : icon_false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleIcon(bool state)
    {
        if (!_image || !icon_true || !icon_false)
        {
            Debug.Log("missing sound icon");
            return;
        }

        _image.sprite = (state) ? icon_true : icon_false;
    }
}
