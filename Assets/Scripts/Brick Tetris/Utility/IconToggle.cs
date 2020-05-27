using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    public Sprite sound_on;
    public Sprite sound_off;

    public bool defaultSoundState = true;

    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = (defaultSoundState) ? sound_on : sound_off;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleIcon(bool state)
    {
        if (!_image || !sound_on || !sound_off)
        {
            Debug.Log("missing sound icon");
            return;
        }

        _image.sprite = (state) ? sound_on : sound_off;
    }
}
