using UnityEngine;

public class StaticMaterial : IMaterial {
    private Material material;

    public StaticMaterial(Texture2D texture) {
        material = new Material(Shader.Find("Unlit/Transparent"));
        material.mainTexture = texture;
    }

    public Material Material => material;
}
