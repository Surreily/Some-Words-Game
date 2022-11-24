using UnityEngine;

public class TileManager : MonoBehaviour, ITile {
    private static readonly Vector3 TileCharacterPosition = new Vector3(1f / 16f * 4.5f, 1f / 16f * 1.5f, 9f);
    private static readonly Vector3 TileBackgroundPosition = new Vector3(0f, 0f, 10f);

    private MovableBehaviour movableBehaviour;
    private TileBackgroundRenderer characterRenderer;
    private TileBackgroundRenderer backgroundRenderer;

    public MaterialStore MaterialStore { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public char Character { get; set; }

    public void Start() {
        SetUpMovableBehaviour();
        SetUpCharacterRenderer();
        SetUpBackgroundRenderer();
    }

    private void SetUpMovableBehaviour() {
        movableBehaviour = gameObject.AddComponent<MovableBehaviour>();
        movableBehaviour.speed = 15f;
        movableBehaviour.distance = 1f;
    }

    private void SetUpCharacterRenderer() {
        characterRenderer = gameObject.AddComponent<TileBackgroundRenderer>();
        characterRenderer.Position = TileCharacterPosition;
        characterRenderer.Width = 1f;
        characterRenderer.Height = 1f;
        characterRenderer.Material = MaterialStore.GetRainbowFontMaterial(Character);
    }

    private void SetUpBackgroundRenderer() {
        backgroundRenderer = gameObject.AddComponent<TileBackgroundRenderer>();
        backgroundRenderer.Position = TileBackgroundPosition;
        backgroundRenderer.Width = 0.95f;
        backgroundRenderer.Height = 0.95f;
        backgroundRenderer.Material = MaterialStore.ImmovableItemBackgroundMaterial;
    }

    public void SetDefaultBackground() {
        backgroundRenderer.Material = MaterialStore.DefaultTileBackgroundMaterial;
    }

    public void SetMatchedBackground() {
        backgroundRenderer.Material = MaterialStore.MatchedItemBackgroundMaterial;
    }

    public void SetImmovableBackground() {
        backgroundRenderer.Material = MaterialStore.ImmovableItemBackgroundMaterial;
    }

    public void Move(Direction direction) {
        movableBehaviour.Move(direction);
    }
}
