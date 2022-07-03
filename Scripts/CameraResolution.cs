using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / (9.0f / 16.0f); //(가로 / 세로)
        float scaleWidth = 1.0f / scaleHeight;

        if( scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        cam.rect = rect;
    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);
}
