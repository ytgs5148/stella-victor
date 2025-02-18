using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Texture2D sourceTexture;
    public Image targetImage;
    public int cropWidth = 1920;
    public int cropHeight = 1080;

    void Start()
    {
        int maxX = sourceTexture.width - cropWidth;
        int maxY = sourceTexture.height - cropHeight;

        int randomX = Random.Range(0, maxX + 1);
        int randomY = Random.Range(0, maxY + 1);

        Rect cropRect = new Rect(randomX, randomY, cropWidth, cropHeight);

        Sprite croppedSprite = Sprite.Create(sourceTexture, cropRect, new Vector2(0.5f, 0.5f));

        targetImage.sprite = croppedSprite;
    }
}
