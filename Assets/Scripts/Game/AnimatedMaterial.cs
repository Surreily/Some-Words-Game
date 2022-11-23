using UnityEngine;

public class AnimatedMaterial : IMaterial {
    private Material material;
    private Texture2D[] frames;

    public AnimatedMaterial(Texture2D[] frames) {
        this.frames = frames;

        material = new Material(Shader.Find("Unlit/Transparent"));
        material.mainTexture = frames[0];
    }

    public Material Material => material;

    

    public void RegisterTimer(GlobalTimer timer) {
        timer.FrameChanged += OnFrameChanged;
    }

    public void UnregisterTimer(GlobalTimer timer) {
        timer.FrameChanged -= OnFrameChanged;
    }

    private void OnFrameChanged(object sender, int e) {
        material.mainTexture = frames[e % frames.Length];
    }
}
