using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class LevelMaterialStore {
        private readonly GlobalTimer timer;

        private Dictionary<SquareTileSetPosition, IMaterial> backgroundMaterialDictionary;
        private IMaterial defaultTileMaterial;
        private IMaterial matchedTileMaterial;
        private IMaterial immovableTileMaterial;

        public LevelMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpBackgroundMaterials();
            SetUpTileMaterials();
        }

        public Material GetBackgroundMaterial(SquareTileSetPosition position) {
            return backgroundMaterialDictionary[position].Material;
        }

        public Material GetDefaultTileMaterial() {
            return defaultTileMaterial.Material;
        }

        public Material GetMatchedTileMaterial() {
            return matchedTileMaterial.Material;
        }

        public Material GetImmovableTileMaterial() {
            return immovableTileMaterial.Material;
        }

        private void SetUpBackgroundMaterials() {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Level Background");

            backgroundMaterialDictionary = new Dictionary<SquareTileSetPosition, IMaterial> {
                { SquareTileSetPosition.TopLeft, MaterialHelper.SetUpStaticMaterial(sprites[0]) },
                { SquareTileSetPosition.Top, MaterialHelper.SetUpStaticMaterial(sprites[1]) },
                { SquareTileSetPosition.TopRight, MaterialHelper.SetUpStaticMaterial(sprites[2]) },
                { SquareTileSetPosition.Left, MaterialHelper.SetUpStaticMaterial(sprites[3]) },
                { SquareTileSetPosition.Center, MaterialHelper.SetUpStaticMaterial(sprites[4]) },
                { SquareTileSetPosition.Right, MaterialHelper.SetUpStaticMaterial(sprites[5]) },
                { SquareTileSetPosition.BottomLeft, MaterialHelper.SetUpStaticMaterial(sprites[6]) },
                { SquareTileSetPosition.Bottom, MaterialHelper.SetUpStaticMaterial(sprites[7]) },
                { SquareTileSetPosition.BottomRight, MaterialHelper.SetUpStaticMaterial(sprites[8]) }
            };
        }

        private void SetUpTileMaterials() {
            defaultTileMaterial = MaterialHelper.SetUpAnimatedMaterial(
                Resources.LoadAll<Sprite>("Sprites/Lines"), timer);
            matchedTileMaterial = MaterialHelper.SetUpAnimatedMaterial(
                Resources.LoadAll<Sprite>("Sprites/Squares"), timer);
            immovableTileMaterial = MaterialHelper.SetUpAnimatedMaterial(
                Resources.LoadAll<Sprite>("Sprites/Static"), timer);
        }
    }
}
