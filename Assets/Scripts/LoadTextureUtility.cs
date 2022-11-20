using System.IO;
using UnityEngine;

public class LoadTextureUtility : MonoBehaviour {
    public Material LoadTexture(string filePath) {
        Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        byte[] bytes;

        using (Stream stream = File.OpenRead(filePath)) {
            bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
        }

        texture.LoadImage(bytes);

        Material material = new Material(Shader.Find("Unlit/Transparent"));
        material.mainTexture = texture;

        return material;
    }
}
