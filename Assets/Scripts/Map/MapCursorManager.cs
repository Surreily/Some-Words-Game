using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapCursorManager : MonoBehaviour {
        private TileRenderer tileRenderer;

        public MaterialStore MaterialStore { get; set; }

        public void Start() {
            tileRenderer = gameObject.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.Ui.GetCursorMaterial();
            tileRenderer.Size = 2f;
        }
    }
}
