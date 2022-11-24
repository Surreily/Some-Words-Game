using System.IO;
using UnityEngine;

public static class LoadTextureUtility {
    public static Texture2D LoadTexture(string filePath) {
        Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        byte[] bytes;

        using (Stream stream = File.OpenRead(filePath)) {
            bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
        }

        texture.LoadImage(bytes);
        texture.filterMode = FilterMode.Point;

        return texture;
    }
}
