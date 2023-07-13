using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapPathManager : MonoBehaviour {
        private TileRenderer tileRenderer;

        public MaterialStore MaterialStore { get; set; }

        public int Colour { get; set; }

        public bool IsOpen { get; set; }

        public PathTileType TileType { get; set; }

        public void Start() {
            tileRenderer = gameObject.AddComponent<TileRenderer>();
            UpdateMaterial();
        }

        public void UpdateMaterial() {
            if (IsOpen) {
                tileRenderer.Material = MaterialStore.Map.GetOpenPathMaterial(Colour, TileType);
            } else {
                tileRenderer.Material = MaterialStore.Map.GetClosedPathMaterial(TileType);
            }
        }
    }
}
