using System.Linq;
using UnityEngine;

public static class SpriteHelper {
    public static Texture2D[] CreateTexturesFromSprites(Sprite[] sprites) {
        return sprites
            .Select(s => CreateTextureFromSprite(s))
            .ToArray();
    }

    public static Texture2D CreateTextureFromSprite(Sprite sprite) {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        texture.filterMode = FilterMode.Point;

        var pixels = sprite.texture.GetPixels(
            (int)sprite.rect.x,
            (int)sprite.rect.y,
            (int)sprite.rect.width,
            (int)sprite.rect.height);

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
