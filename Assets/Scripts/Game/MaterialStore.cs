using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialStore {
    private static readonly char[] supportedCharacters = "abcdefghijklmnopqrstuvwxyz".ToArray();

    private GlobalTimer timer;

    private IMaterial borderMaterial;
    private IMaterial cursorMaterial;
    private IMaterial defaultTileBackgroundMaterial;
    private IMaterial matchedTileBackgroundMaterial;
    private IMaterial immovableTileBackgroundMaterial;

    private Dictionary<char, IMaterial> whiteFontMaterialDictionary;
    private Dictionary<char, IMaterial> blackFontMaterialDictionary;
    private Dictionary<char, IMaterial> rainbowFontMaterialDictionary;

    public MaterialStore(GlobalTimer timer) {
        this.timer = timer;

        // TODO: Don't call this in the constructor?
        SetUpMaterials();
        SetUpFonts();
    }

    #region Get Material

    public Material BorderMaterial => borderMaterial.Material;

    public Material CursorMaterial => cursorMaterial.Material;

    public Material DefaultTileBackgroundMaterial => defaultTileBackgroundMaterial.Material;

    public Material MatchedItemBackgroundMaterial => matchedTileBackgroundMaterial.Material;

    public Material ImmovableItemBackgroundMaterial => immovableTileBackgroundMaterial.Material;

    // TODO: GetCursorMaterial()
    // TODO: GetBorderMaterial()
    // TODO: GetSpecialTileHighlightMaterial()
    // TODO: GetImmovableTileHighlightMaterial()

    #endregion

    #region Get Font

    public Material GetWhiteFontMaterial(char character) {
        return whiteFontMaterialDictionary[character].Material;
    }

    public Material GetBlackFontMaterial(char character) {
        return blackFontMaterialDictionary[character].Material;
    }

    public Material GetRainbowFontMaterial(char character) {
        return rainbowFontMaterialDictionary[character].Material;
    }

    #endregion

    #region Texture Setup

    private void SetUpMaterials() {
        borderMaterial = SetUpAnimatedMaterial(Resources.LoadAll<Sprite>("Sprites/Border"));
        cursorMaterial = SetUpAnimatedMaterial(Resources.LoadAll<Sprite>("Sprites/Cursor"));
        defaultTileBackgroundMaterial = SetUpAnimatedMaterial(Resources.LoadAll<Sprite>("Sprites/Lines"));
        matchedTileBackgroundMaterial = SetUpAnimatedMaterial(Resources.LoadAll<Sprite>("Sprites/Squares"));
        immovableTileBackgroundMaterial = SetUpAnimatedMaterial(Resources.LoadAll<Sprite>("Sprites/Static"));
    }

    private StaticMaterial SetUpStaticMaterial(Sprite sprite) {
        Texture2D texture = SpriteHelper.CreateTextureFromSprite(sprite);

        return new StaticMaterial(texture);
    }

    private AnimatedMaterial SetUpAnimatedMaterial(Sprite[] sprites) {
        Texture2D[] textures = sprites
            .Select(s => SpriteHelper.CreateTextureFromSprite(s))
            .ToArray();

        AnimatedMaterial animatedMaterial = new AnimatedMaterial(textures);
        animatedMaterial.RegisterTimer(timer);

        return animatedMaterial;
    }

    #endregion

    #region Font Setup

    private void SetUpFonts() {
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
            sprites.Skip(characterCount * 2).Take(characterCount * 8).ToArray());
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

    #endregion

}
