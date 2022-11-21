using UnityEngine;

public class AnimatedMaterial : IMaterial {
    private Material material;
    private Texture2D[] frames;

    public AnimatedMaterial(GlobalTimer timer, Texture2D[] frames) {
        this.frames = frames;

        material = new Material(Shader.Find("Unlit/Transparent"));
        material.mainTexture = frames[timer.Frame % frames.Length];

        timer.FrameChanged += OnFrameChanged;
    }

    public Material Material => material;

    private void OnFrameChanged(object sender, int e) {
        material.mainTexture = frames[e % frames.Length];
    }
}
