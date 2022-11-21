using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialStore {
    private GlobalTimer timer;
    private Dictionary<string, IMaterial> materials;

    public MaterialStore(GlobalTimer timer) {
        this.timer = timer;

        materials = new Dictionary<string, IMaterial>();
    }

    public void Register(string key, Texture2D texture) {
        materials.Add(key, new StaticMaterial(texture));
    }

    public void Register(string key, params Texture2D[] textures) {
        materials.Add(key, new AnimatedMaterial(timer, textures));
    }

    public void Register(string key, Sprite[] sprites) {
        Texture2D[] textures = new Texture2D[sprites.Length];

        for (int i = 0; i < sprites.Length; i++) {
            Sprite sprite = sprites[i];

            Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            texture.filterMode = FilterMode.Point;

            var pixels = sprite.texture.GetPixels(
                (int)sprite.rect.x,
                (int)sprite.rect.y,
                (int)sprite.rect.width,
                (int)sprite.rect.height);

            texture.SetPixels(pixels);
            texture.Apply();

            textures[i] = texture;
        }

        Register(key, textures);
    }

    public Material Get(string key) {
        return materials[key].Material;
    }
}
