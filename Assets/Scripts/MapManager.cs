using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public MaterialStore MaterialStore { get; set; }

    public void LoadMap(JsonMap map) {
        foreach (JsonMapDecoration decoration in map.Decorations) {
            for (int x = 0; x < decoration.Width; x++) {
                for (int y = 0; y < decoration.Height; y++) {
                    CreateDecoration(decoration.X + x, decoration.Y + y, decoration.Material);
                }
            }
        }

        foreach (JsonMapPath path in map.Paths) {
            for (int x = 0; x < path.Width; x++) {
                for (int y = 0; y < path.Height; y++) {
                    CreatePath(path.X + x, path.Y + y);
                }
            }
        }

        foreach (JsonMapLevel level in map.Levels) {
            CreateLevel(level.X, level.Y, level.Id);
        }
    }

    private void CreateDecoration(int x, int y, string material) {
        GameObject decoration = new GameObject();
        decoration.transform.Translate(x, y, Layers.MapDecoration, Space.Self);

        TileRenderer tileRenderer = decoration.AddComponent<TileRenderer>();
        tileRenderer.Material = MaterialStore.BorderMaterial; // TODO: Use decoration material.
    }

    private void CreatePath(int x, int y) {
        GameObject path = new GameObject();
        path.transform.Translate(x, y, Layers.MapPath, Space.Self);

        TileRenderer tileRenderer = path.AddComponent<TileRenderer>();
        tileRenderer.Material = MaterialStore.ImmovableItemBackgroundMaterial; // TODO: Use path material.
    }

    private void CreateLevel(int x, int y, string id) {
        GameObject level = new GameObject();
        level.transform.Translate(x, y, Layers.MapLevel, Space.Self);

        TileRenderer tileRenderer = level.AddComponent<TileRenderer>();
        tileRenderer.Material = MaterialStore.GetOpenLevelMaterial(0); // TODO: Pass in the variation.
    }
}
