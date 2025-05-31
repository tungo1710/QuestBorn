using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0.1f, 0f);

    void Update()
    {
        if (rawImage != null)
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + scrollSpeed * Time.deltaTime, rawImage.uvRect.size);
        }
    }
}