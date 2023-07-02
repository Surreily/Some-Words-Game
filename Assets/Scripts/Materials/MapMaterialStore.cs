using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class MapMaterialStore {
        private GlobalTimer timer;

        private Dictionary<(int, PathTileType), IMaterial> pathDictionary;
        private Dictionary<int, IMaterial> openLevelDictionary;
        private Dictionary<int, IMaterial> clearedLevelDictionary;

        public MapMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpPathMaterials();
            SetUpLevelMaterials();
        }

        public Material GetPathMaterial(int variation, PathTileType type) {
            return pathDictionary[(variation, type)].Material;
        }

        public Material GetOpenLevelMaterial(int variation) {
            return openLevelDictionary[variation].Material;
        }

        public Material GetClearedLevelMaterial(int variation) {
            return clearedLevelDictionary[variation].Material;
        }

        private void SetUpPathMaterials() {
            pathDictionary = new Dictionary<(int, PathTileType), IMaterial>();

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Path Tiles");

            for (int i = 0; i < 16; i++) {
                pathDictionary.Add(
                    (i, PathTileType.Horizontal),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11)]));
                pathDictionary.Add(
                    (i, PathTileType.Vertical),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 1]));
                pathDictionary.Add(
                    (i, PathTileType.UpAndRight),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 2]));
                pathDictionary.Add(
                    (i, PathTileType.DownAndRight),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 3]));
                pathDictionary.Add(
                    (i, PathTileType.DownAndLeft),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 4]));
                pathDictionary.Add(
                    (i, PathTileType.UpAndLeft),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 5]));
                pathDictionary.Add(
                    (i, PathTileType.HorizontalAndUp),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 6]));
                pathDictionary.Add(
                    (i, PathTileType.VerticalAndRight),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 7]));
                pathDictionary.Add(
                    (i, PathTileType.HorizontalAndDown),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 8]));
                pathDictionary.Add(
                    (i, PathTileType.VerticalAndLeft),
                    MaterialHelper.SetUpStaticMaterial(sprites[(i * 11) + 9]));
                pathDictionary.Add(
                    (i, PathTileType.All),
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
