using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapLevelManager : MonoBehaviour {
        private TileRenderer tileRenderer;

        public MaterialStore MaterialStore { get; set; }

        public int Variation { get; set; }

        public bool IsOpen { get; set; }

        public void Start() {
            tileRenderer = gameObject.AddComponent<TileRenderer>();
            UpdateMaterial();
        }

        public void UpdateMaterial() {
            if (IsOpen) {
                tileRenderer.Material = MaterialStore.Map.GetOpenLevelMaterial(Variation);
            } else {
                tileRenderer.Material = MaterialStore.Map.GetOpenLevelMaterial(Variation); // TODO: Get closed level material.
            }
        }
    }
}
