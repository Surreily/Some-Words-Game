using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapCursorManager : MonoBehaviour {
        private TileRenderer tileRenderer;
        private Vector3? target;

        public MaterialStore MaterialStore { get; set; }

        public bool IsMoving => target != null;

        public void Start() {
            tileRenderer = gameObject.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.Ui.GetCursorMaterial();
            tileRenderer.Size = 2f;
        }

        public void Update() {
            if (target.HasValue) {
                transform.position = Vector3.MoveTowards(transform.position, target.Value, Time.deltaTime * 10);

                if (transform.position == target) {
                    target = null;
                }
            }
        }

        public void Move(int x, int y) {
            target = new Vector3(x, y, 0f);
        }
    }
}
