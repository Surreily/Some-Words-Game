using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class MaterialStore {
        private GlobalTimer timer;

        private IMaterial cursorMaterial;

        public MaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpMaterials();

            Font = new FontMaterialStore(timer);
            Map = new MapMaterialStore(timer);
            Level = new LevelMaterialStore(timer);
        }

        public FontMaterialStore Font { get; }
        public MapMaterialStore Map { get; }
        public LevelMaterialStore Level { get; }

        public Material CursorMaterial => cursorMaterial.Material;

        private void SetUpMaterials() {
            cursorMaterial = MaterialHelper.SetUpAnimatedMaterial(
                Resources.LoadAll<Sprite>("Sprites/Cursor"), timer);
        }
    }
}