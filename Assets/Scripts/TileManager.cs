using System;
using Surreily.SomeWords.Scripts.Materials;
using TMPro;
using UnityEngine;

public class Tile : ITile {
    private readonly MaterialStore materialStore;
    private readonly MovableBehaviour movableBehaviour;
    private readonly PulseAnimationBehaviour pulseAnimationBehaviour;
    private readonly TMP_Text textMeshProText;
    private readonly SpriteRenderer backgroundRenderer;

    private TileState tileState;

    public Tile(
        MaterialStore materialStore,
        MovableBehaviour movableBehaviour,
        PulseAnimationBehaviour pulseAnimationBehaviour,
        TMP_Text textMeshProText,
        SpriteRenderer backgroundRenderer) {

        this.materialStore = materialStore;
        this.movableBehaviour = movableBehaviour;
        this.pulseAnimationBehaviour = pulseAnimationBehaviour;
        this.textMeshProText = textMeshProText;
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
        textMeshProText.color = Color.white;
        backgroundRenderer.sprite = materialStore.Level.DefaultTileSprite;
    }

    private void SetValidState() {
        textMeshProText.color = Color.green; // TODO: Rainbow?
        backgroundRenderer.sprite = materialStore.Level.MatchedTileSprite;
    }

    private void SetInvalidState() {
        textMeshProText.color = Color.red;
        backgroundRenderer.sprite = materialStore.Level.ImmovableTileSprite;
    }
}
