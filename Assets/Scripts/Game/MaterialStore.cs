using System.Collections.Generic;
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

    public Material Get(string key) {
        return materials[key].Material;
    }
}
