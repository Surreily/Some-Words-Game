using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapLevelManager : MonoBehaviour {
        private TileRenderer tileRenderer;

        public MaterialStore MaterialStore { get; set; }

        public int Colour { get; set; }

        public bool IsOpen { get; set; }

        public void Start() {
            tileRenderer = gameObject.AddComponent<TileRenderer>();
            UpdateMaterial();
        }

        public void UpdateMaterial() {
            if (IsOpen) {
                tileRenderer.Material = MaterialStore.Map.GetOpenLevelMaterial(Colour);
            } else {
                tileRenderer.Material = MaterialStore.Map.GetOpenLevelMaterial(Colour); // TODO: Get closed level material.
            }
        }
    }
}
