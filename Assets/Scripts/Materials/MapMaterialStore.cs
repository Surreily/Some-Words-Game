using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class MapMaterialStore {
        private const PathTileType AllPathTileType =
            PathTileType.Up | PathTileType.Right | PathTileType.Down | PathTileType.Left;

        private GlobalTimer timer;

        private Sprite levelSprite;

        private Dictionary<(int, PathTileType), IMaterial> openPathDictionary;
        private Dictionary<PathTileType, IMaterial> closedPathDictionary;
        private Dictionary<int, IMaterial> openLevelDictionary;
        private Dictionary<int, IMaterial> clearedLevelDictionary;

        private Dictionary<PathTileType, Sprite> pathSpriteDictionary;

        public MapMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpPathSprites();
            SetUpLevelSprites();

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

        public Sprite GetPathSprite(PathTileType type) {
            if (pathSpriteDictionary.TryGetValue(type, out Sprite sprite)) {
                return sprite;
            }

            return pathSpriteDictionary[AllPathTileType];
        }

        public Sprite GetLevelSprite() {
            return levelSprite;
        }

        public Material GetOpenLevelMaterial(int colour) {
            return openLevelDictionary[colour].Material;
        }

        public Material GetClearedLevelMaterial(int colour) {
            return clearedLevelDictionary[colour].Material;
        }

        private void SetUpPathSprites() {
            pathSpriteDictionary = new Dictionary<PathTileType, Sprite>();

            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Path Tiles");

            pathSpriteDictionary.Add(
                PathTileType.Right | PathTileType.Left,
                sprites[0]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Down,
                sprites[1]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Right,
                sprites[2]);
            pathSpriteDictionary.Add(
                PathTileType.Right | PathTileType.Down,
                sprites[3]);
            pathSpriteDictionary.Add(
                PathTileType.Down | PathTileType.Left,
                sprites[4]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Left,
                sprites[5]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Right | PathTileType.Left,
                sprites[6]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Right | PathTileType.Down,
                sprites[7]);
            pathSpriteDictionary.Add(
                PathTileType.Right | PathTileType.Down | PathTileType.Left,
                sprites[8]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Down | PathTileType.Left,
                sprites[9]);
            pathSpriteDictionary.Add(
                PathTileType.Up | PathTileType.Right | PathTileType.Down | PathTileType.Left,
                sprites[10]);
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

        // TODO: This just loads the first sprite on the sprite sheet!
        private void SetUpLevelSprites() {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Level Tiles");

            levelSprite = sprites[0];
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
