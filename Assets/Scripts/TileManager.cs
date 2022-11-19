using UnityEngine;

public class TileManager : MonoBehaviour, ITile {
    [SerializeField]
    public Material CharacterMaterial;

    [SerializeField]
    public Material BackgroundMaterial;

    [SerializeField]
    public char character;

    private MovableBehaviour movableBehaviour;

    public int X { get; set; }
    public int Y { get; set; }

    public TileCharacterRenderer CharacterRenderer { get; private set; }
    public AnimatedTextureRenderer BackgroundRenderer { get; private set; }

    public char? Character => character;

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
        CharacterRenderer = gameObject.AddComponent<TileCharacterRenderer>();
        CharacterRenderer.character = character;
        CharacterRenderer.material = CharacterMaterial;
    }

    private void SetUpBackgroundRenderer() {
        BackgroundRenderer = gameObject.AddComponent<AnimatedTextureRenderer>();
        BackgroundRenderer.width = 0.95f;
        BackgroundRenderer.height = 0.95f;
        BackgroundRenderer.z = 10f;
        BackgroundRenderer.material = BackgroundMaterial;
    }

    public void Move(Direction direction) {
        movableBehaviour.Move(direction);
    }
}
