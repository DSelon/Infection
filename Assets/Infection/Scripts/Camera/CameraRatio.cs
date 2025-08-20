using UnityEngine;

public class CameraRatio : MonoBehaviour
{
    private int width = 16;
    private int height = 9;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float ScaleWidth;
        float ScaleHeight;

        ScaleHeight = ((float)Screen.width / Screen.height) / ((float)width / height);
        ScaleWidth = 1.0f / ScaleHeight;
        if (ScaleHeight < 1)
        {
            rect.height = ScaleHeight;
            rect.y = (1.0f - ScaleHeight) / 2.0f;
        }
        else
        {
            rect.width = ScaleWidth;
            rect.x = (1.0f - ScaleWidth) / 2.0f;
        }
        camera.rect = rect;
    }
}
