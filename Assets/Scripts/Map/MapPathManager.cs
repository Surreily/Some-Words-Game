using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapPathManager : MonoBehaviour {
        private TileRenderer tileRenderer;

        public MaterialStore MaterialStore { get; set; }

        public int Variation { get; set; }

        public bool IsOpen { get; set; }

        public PathTileType TileType { get; set; }

        public void Start() {
            tileRenderer = gameObject.AddComponent<TileRenderer>();
            UpdateMaterial();
        }

        public void UpdateMaterial() {
            if (IsOpen) {
                tileRenderer.Material = MaterialStore.Map.GetPathMaterial(Variation, TileType);
            } else {
                tileRenderer.Material = MaterialStore.Map.GetPathMaterial(Variation, TileType); // TODO: Get closed path.
            }
        }
    }
}
