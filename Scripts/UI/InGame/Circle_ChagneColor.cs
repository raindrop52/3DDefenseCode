using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle_ChagneColor : MonoBehaviour
{
    Image _img;

    void Start()
    {
        _img = GetComponent<Image>();
    }
        
    public void ChangeColor(Color color)
    {
        _img.color = color;
    }
}
