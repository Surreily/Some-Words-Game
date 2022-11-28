using System;

public class Tile : ITile {
    private MaterialStore materialStore;

    private MovableBehaviour movableBehaviour;
    private PulseAnimationBehaviour pulseAnimationBehaviour;
    private TileBackgroundRenderer characterRenderer;
    private TileBackgroundRenderer backgroundRenderer;

    private TileState tileState;

    public Tile(
        MaterialStore materialStore,
        MovableBehaviour movableBehaviour,
        PulseAnimationBehaviour pulseAnimationBehaviour,
        TileBackgroundRenderer characterRenderer,
        TileBackgroundRenderer backgroundRenderer) {

        this.materialStore = materialStore;
        this.movableBehaviour = movableBehaviour;
        this.pulseAnimationBehaviour = pulseAnimationBehaviour;
        this.characterRenderer = characterRenderer;
        this.backgroundRenderer = backgroundRenderer;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public char Character { get; set; }

    public TileState TileState {
        get { return tileState; }
        set {
            tileState = value;

            switch(value) {
                case TileState.Normal:
                    SetNormalState();
                    break;
                case TileState.Valid:
                    SetValidState();
                    break;
                case TileState.Invalid:
                    SetInvalidState();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported TileState.");
            }
        }
    }

    public void AnimateMove(Direction direction) {
        movableBehaviour.Move(direction);
    }

    public void AnimatePulse() {
        pulseAnimationBehaviour.Pulse();
    }

    private void SetNormalState() {
        characterRenderer.Material = materialStore.GetWhiteFontMaterial(Character);
        backgroundRenderer.Material = materialStore.DefaultTileBackgroundMaterial;
    }

    private void SetValidState() {
        characterRenderer.Material = materialStore.GetRainbowFontMaterial(Character);
        backgroundRenderer.Material = materialStore.MatchedItemBackgroundMaterial;
    }

    private void SetInvalidState() {
        characterRenderer.Material = materialStore.GetBlackFontMaterial(Character); // TODO: Make this red.
        backgroundRenderer.Material = materialStore.ImmovableItemBackgroundMaterial;
    }
}
