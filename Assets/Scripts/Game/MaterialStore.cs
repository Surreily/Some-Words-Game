using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

public class MaterialStore {
    private static readonly char[] supportedCharacters = "abcdefghijklmnopqrstuvwxyz".ToArray();

    private GlobalTimer timer;

    private IMaterial borderMaterial;
    private IMaterial cursorMaterial;
    private IMaterial defaultTileBackgroundMaterial;
    private IMaterial matchedTileBackgroundMaterial;
    private IMaterial immovableTileBackgroundMaterial;

    private Dictionary<int, IMaterial> OpenLevelDictionary;
    private Dictionary<int, IMaterial> ClearedLevelDictionary;
    private Dictionary<SquareTileSetPosition, IMaterial> levelBackgroundMaterialDictionary; 
    private Dictionary<char, IMaterial> whiteFontMaterialDictionary;
    private Dictionary<char, IMaterial> blackFontMaterialDictionary;
    private Dictionary<char, IMaterial> rainbowFontMaterialDictionary;

    public MaterialStore(GlobalTimer timer) {
        this.timer = timer;

        // TODO: Don't call this in the constructor?
        SetUpMaterials();
        SetUpMapLevelTileMaterials();
        SetUpLevelBackgroundMaterials();
        SetUpFonts();
    }

    #region Get Material

    public Material BorderMaterial => borderMaterial.Material;

    public Material CursorMaterial => cursorMaterial.Material;

    public Material DefaultTileBackgroundMaterial => defaultTileBackgroundMaterial.Material;

    public Material MatchedItemBackgroundMaterial => matchedTileBackgroundMaterial.Material;

    public Material ImmovableItemBackgroundMaterial => immovableTileBackgroundMaterial.Material;

    public Material GetOpenLevelMaterial(int variation) {
        return OpenLevelDictionary[variation].Material;
    }

    public Material GetClearedLevelMaterial(int variation) {
        return ClearedLevelDictionary[variation].Material;
    }

    public Material GetLevelBackgroundMaterial(SquareTileSetPosition position) {
        return levelBackgroundMaterialDictionary[position].Material;
    }

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

    private void SetUpMapLevelTileMaterials() {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Map Level Tiles");

        ClearedLevelDictionary = new Dictionary<int, IMaterial>();
        OpenLevelDictionary = new Dictionary<int, IMaterial>();

        for (int i = 0; i < 16; i++) {
            ClearedLevelDictionary.Add(i, SetUpAnimatedMaterial(
                sprites.Skip(i * 8).Take(4).ToArray()));
            OpenLevelDictionary.Add(i, SetUpAnimatedMaterial(
                sprites.Skip((i * 8) + 4).Take(4).ToArray()));
        }
    }

    private void SetUpLevelBackgroundMaterials() {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Level Background");

        levelBackgroundMaterialDictionary = new Dictionary<SquareTileSetPosition, IMaterial> {
            { SquareTileSetPosition.TopLeft, SetUpStaticMaterial(sprites[0]) },
            { SquareTileSetPosition.Top, SetUpStaticMaterial(sprites[1]) },
            { SquareTileSetPosition.TopRight, SetUpStaticMaterial(sprites[2]) },
            { SquareTileSetPosition.Left, SetUpStaticMaterial(sprites[3]) },
            { SquareTileSetPosition.Center, SetUpStaticMaterial(sprites[4]) },
            { SquareTileSetPosition.Right, SetUpStaticMaterial(sprites[5]) },
            { SquareTileSetPosition.BottomLeft, SetUpStaticMaterial(sprites[6]) },
            { SquareTileSetPosition.Bottom, SetUpStaticMaterial(sprites[7]) },
            { SquareTileSetPosition.BottomRight, SetUpStaticMaterial(sprites[8]) }
        };
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
