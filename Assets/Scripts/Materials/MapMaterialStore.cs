using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class MapMaterialStore {
        private GlobalTimer timer;

        private Dictionary<(int, PathTileSetPosition), IMaterial> pathDictionary;
        private Dictionary<int, IMaterial> openLevelDictionary;
        private Dictionary<int, IMaterial> clearedLevelDictionary;

        public MapMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpPathMaterials();
            SetUpLevelMaterials();
        }

        public Material GetPathMaterial(int variation, PathTileSetPosition type) {
            return pathDictionary[(variation, type)].Material;
        }

        public Material GetOpenLevelMaterial(int variation) {
            return openLevelDictionary[variation].Material;
        }

        public Material GetClearedLevelMaterial(int variation) {
            return clearedLevelDictionary[variation].Material;
        }

        private void SetUpPathMaterials() {
            pathDictionary = new Dictionary<(int, PathTileSetPosition), IMaterial>();

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Path Tiles");

            for (int i = 0; i < 16; i++) {
                pathDictionary.Add(
                    (i, PathTileSetPosition.Horizontal),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11)]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.Vertical),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 1]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.UpAndRight),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 2]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.DownAndRight),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 3]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.DownAndLeft),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 4]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.UpAndLeft),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 5]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.HorizontalAndUp),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 6]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.VerticalAndRight),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 7]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.HorizontalAndDown),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 8]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.VerticalAndLeft),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 9]));
                pathDictionary.Add(
                    (i, PathTileSetPosition.All),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 10]));
            }
        }

        private void SetUpLevelMaterials() {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Level Tiles");

            clearedLevelDictionary = new Dictionary<int, IMaterial>();
            openLevelDictionary = new Dictionary<int, IMaterial>();

            for (int i = 0; i < 16; i++) {
                clearedLevelDictionary.Add(i, MaterialHelper.SetUpAnimatedMaterial(
                    sprites.Skip(i * 8).Take(4).ToArray(), timer));
                openLevelDictionary.Add(i, MaterialHelper.SetUpAnimatedMaterial(
                    sprites.Skip((i * 8) + 4).Take(4).ToArray(), timer));
            }
        }
    }
}
