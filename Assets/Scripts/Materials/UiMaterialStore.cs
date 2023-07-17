using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class UiMaterialStore {
        private readonly GlobalTimer timer;

        private IMaterial cursorMaterial;

        public Sprite CursorSprite { get; private set; }

        public UiMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpCursorMaterial();
        }

        public Material GetCursorMaterial() {
            return cursorMaterial.Material;
        }

        private void SetUpCursorMaterial() {
            Sprite sprite = Resources.Load<Sprite>("Sprites/Cursor");

            cursorMaterial = MaterialHelper.SetUpStaticMaterial(sprite);

            CursorSprite = Resources.Load<Sprite>("Sprites/Cursor");
        }
    }
}
