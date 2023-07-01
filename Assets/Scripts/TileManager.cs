using System;
using Surreily.SomeWords.Scripts.Materials;

public class Tile : ITile {
    private readonly MaterialStore materialStore;
    private readonly MovableBehaviour movableBehaviour;
    private readonly PulseAnimationBehaviour pulseAnimationBehaviour;
    private readonly TileBackgroundRenderer characterRenderer;
    private readonly TileBackgroundRenderer backgroundRenderer;

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
        characterRenderer.Material = materialStore.Font.GetWhiteFontMaterial(Character);
        backgroundRenderer.Material = materialStore.Level.GetDefaultTileMaterial();
    }

    private void SetValidState() {
        characterRenderer.Material = materialStore.Font.GetRainbowFontMaterial(Character);
        backgroundRenderer.Material = materialStore.Level.GetMatchedTileMaterial();
    }

    private void SetInvalidState() {
        characterRenderer.Material = materialStore.Font.GetBlackFontMaterial(Character); // TODO: Make this red.
        backgroundRenderer.Material = materialStore.Level.GetImmovableTileMaterial();
    }
}
