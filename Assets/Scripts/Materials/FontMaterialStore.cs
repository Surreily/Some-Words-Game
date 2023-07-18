using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Materials {
    public class FontMaterialStore {
        private static readonly char[] supportedCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!\"#$%&'()*+,-./:;<=>?".ToArray();

        private readonly GlobalTimer timer;

        private Dictionary<char, IMaterial> whiteFontMaterialDictionary;
        private Dictionary<char, IMaterial> blackFontMaterialDictionary;
        private Dictionary<char, IMaterial> rainbowFontMaterialDictionary;

        public TMP_FontAsset VgaFont { get; private set; }

        public FontMaterialStore(GlobalTimer timer) {
            this.timer = timer;

            SetUpFontMaterials();

            VgaFont = Resources.Load<TMP_FontAsset>("Fonts/VGA Font");
        }

        public Material GetWhiteFontMaterial(char character) {
            return whiteFontMaterialDictionary[character].Material;
        }

        public Material GetBlackFontMaterial(char character) {
            return blackFontMaterialDictionary[character].Material;
        }

        public Material GetRainbowFontMaterial(char character) {
            return rainbowFontMaterialDictionary[character].Material;
        }

        private void SetUpFontMaterials() {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/VGA Font");

            int characterCount = supportedCharacters.Length;

            whiteFontMaterialDictionary = SetUpStaticFont(
                supportedCharacters,
                sprites.Take(characterCount).ToArray());

            blackFontMaterialDictionary = SetUpStaticFont(
                supportedCharacters,
                sprites.Skip(characterCount).Take(characterCount).ToArray());

            rainbowFontMaterialDictionary = SetUpAnimatedFont(
                supportedCharacters,
                sprites.Skip(characterCount * 2).Take(characterCount * 16).ToArray());
        }

        private Dictionary<char, IMaterial> SetUpStaticFont(char[] characters, Sprite[] sprites) {
            int characterCount = characters.Length;

            Dictionary<char, IMaterial> materials = new Dictionary<char, IMaterial>();

            for (int characterIndex = 0; characterIndex < characterCount; characterIndex++) {
                char character = characters[characterIndex];

                materials.Add(character, new StaticMaterial(SpriteHelper.CreateTextureFromSprite(sprites[characterIndex])));
            }

            return materials;
        }

        private Dictionary<char, IMaterial> SetUpAnimatedFont(char[] characters, Sprite[] sprites) {
            int characterCount = characters.Length;
            int frameCount = sprites.Length / characterCount;

            Dictionary<char, IMaterial> materials = new Dictionary<char, IMaterial>();

            for (int characterIndex = 0; characterIndex < characterCount; characterIndex++) {
                char character = characters[characterIndex];

                Texture2D[] frames = new Texture2D[frameCount];

                for (int frameIndex = 0; frameIndex < frameCount; frameIndex++) {
                    int spriteIndex = (frameIndex * characterCount) + characterIndex;
                    frames[frameIndex] = SpriteHelper.CreateTextureFromSprite(sprites[spriteIndex]);
                }

                AnimatedMaterial material = new AnimatedMaterial(frames);
                material.RegisterTimer(timer);

                materials.Add(character, material);
            }

            return materials;
        }
    }
}
