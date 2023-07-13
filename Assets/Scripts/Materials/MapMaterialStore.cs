using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class MapMaterialStore {
        private const PathTileType AllPathTileType =
            PathTileType.Up | PathTileType.Right | PathTileType.Down | PathTileType.Left;

        private GlobalTimer timer;

        private Dictionary<(int, PathTileType), IMaterial> openPathDictionary;
        private Dictionary<PathTileType, IMaterial> closedPathDictionary;
        private Dictionary<int, IMaterial> openLevelDictionary;
        private Dictionary<int, IMaterial> clearedLevelDictionary;

        public MapMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpPathMaterials();
            SetUpLevelMaterials();
        }

        public Material GetOpenPathMaterial(int colour, PathTileType type) {
            if (openPathDictionary.TryGetValue((colour, type), out IMaterial material)) {
                return material.Material;
            }

            return openPathDictionary[(colour, AllPathTileType)].Material;
        }

        public Material GetClosedPathMaterial(PathTileType type) {
            if (closedPathDictionary.TryGetValue(type, out IMaterial material)) {
                return material.Material;
            }

            return closedPathDictionary[AllPathTileType].Material;
        }

        public Material GetOpenLevelMaterial(int colour) {
            return openLevelDictionary[colour].Material;
        }

        public Material GetClearedLevelMaterial(int colour) {
            return clearedLevelDictionary[colour].Material;
        }

        private void SetUpPathMaterials() {
            openPathDictionary = new Dictionary<(int, PathTileType), IMaterial>();

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Path Tiles");

            int spriteIndex = 0;

            for (int i = 0; i < 16; i++) {
                openPathDictionary.Add(
                    (i, PathTileType.Right | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Down),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Right),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Right | PathTileType.Down),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Down | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Right | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Right | PathTileType.Down),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Right | PathTileType.Down | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Down | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
                openPathDictionary.Add(
                    (i, PathTileType.Up | PathTileType.Right | PathTileType.Down | PathTileType.Left),
                    MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            }

            closedPathDictionary = new Dictionary<PathTileType, IMaterial>();

            closedPathDictionary.Add(
                PathTileType.Right | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Down,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Right,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Right | PathTileType.Down,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Down | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Right | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Right | PathTileType.Down,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Right | PathTileType.Down | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Down | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
            closedPathDictionary.Add(
                PathTileType.Up | PathTileType.Right | PathTileType.Down | PathTileType.Left,
                MaterialHelper.SetUpStaticMaterial(sprites[spriteIndex++]));
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
