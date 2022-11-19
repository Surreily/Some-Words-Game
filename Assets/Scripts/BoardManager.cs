using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour, IBoard {
    [SerializeField]
    public Material characterMaterial;

    [SerializeField]
    public Material backgroundMaterial;

    [SerializeField]
    public Material cursorMaterial;

    [SerializeField]
    public AudioClip cursorAudioClip;

    [SerializeField]
    public AudioClip interactAudioClip;

    [SerializeField]
    public AudioClip invalidAudioClip;

    private GameObject cursor;
    private MovableBehaviour cursorMovableBehaviour;

    private Stack<IAction> actions;

    public int Width { get; set; }
    public int Height { get; set; }
    public int CursorX { get; set; }
    public int CursorY { get; set; }
    public ITile[,] Tiles { get; set; }
    public ITile SelectedTile { get; set; }
    

    public void LoadBoard(JsonLevel level) {
        actions = new Stack<IAction>();

        Width = level.Width;
        Height = level.Height;
        CursorX = level.CursorX;
        CursorY = level.CursorY;
        Tiles = new ITile[level.Width, level.Height];

        foreach (JsonTile tile in level.Tiles) {
            AddTile(tile.Character[0], tile.X, tile.Y);
        }

        SetUpCursor(level.CursorX, level.CursorY);
    }

    private void SetUpCursor(int x, int y) {
        cursor = new GameObject();
        cursor.transform.SetParent(transform, false);
        cursor.transform.position = new Vector3(x, y, 0f);

        cursorMovableBehaviour = cursor.AddComponent<MovableBehaviour>();
        cursorMovableBehaviour.speed = 15f;
        cursorMovableBehaviour.distance = 1f;

        TextureRenderer textureRenderer = cursor.AddComponent<TextureRenderer>();
        textureRenderer.material = cursorMaterial;
        textureRenderer.width = 1.05f;
        textureRenderer.height = 1.05f;
        textureRenderer.z = 5f;
    }

    private void AddTile(char c, int x, int y) {
        GameObject tile = new GameObject() {
            name = "Tile " + c,
        };

        tile.transform.SetParent(transform, false);
        tile.transform.position = new Vector3(x, y, 0f);

        TileManager tileManager = tile.AddComponent<TileManager>();
        tileManager.CharacterMaterial = characterMaterial;
        tileManager.BackgroundMaterial = backgroundMaterial;
        tileManager.character = c;

        Tiles[x, y] = tileManager;
    }

    private void DoAction(IAction action) {
        if (action.Do()) {
            actions.Push(action);
        }
    }

    private void UndoAction() {
        if (actions.TryPop(out IAction action)) {
            action.Undo();
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            DoAction(new MoveCursorAction(this, Direction.Left));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            DoAction(new MoveCursorAction(this, Direction.Right));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            DoAction(new MoveCursorAction(this, Direction.Up));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            DoAction(new MoveCursorAction(this, Direction.Down));
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            DoAction(new PickUpOrDropTileAction(this));
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            UndoAction();
        }
    }

    public void MoveCursor(Direction direction) {
        switch (direction) {
            case Direction.Left:
                CursorX--;
                break;
            case Direction.Right:
                CursorX++;
                break;
            case Direction.Up:
                CursorY++;
                break;
            case Direction.Down:
                CursorY--;
                break;
            default:
                throw new ArgumentException("Unsupported direction.", nameof(direction));
        }

        cursorMovableBehaviour.Move(direction);
    }

    public void PlayCursorAudioClip() {
        AudioSource.PlayClipAtPoint(cursorAudioClip, Vector3.zero);
    }

    public void PlayInteractAudioClip() {
        AudioSource.PlayClipAtPoint(interactAudioClip, Vector3.zero);
    }

    public void PlayInvalidAudioClip() {
        AudioSource.PlayClipAtPoint(invalidAudioClip, Vector3.zero);
    }

    ////private void HandleSelect() {
    ////    if (selectedTile == null) {
    ////        TileManager tile = tiles[CursorX, CursorY];

    ////        if (tile != null) {
    ////            // TODO: Play "picked up" sound
    ////            selectedTile = tile;
    ////        } else {
    ////            // TODO: Play "can't do that" sound
    ////        }
    ////    } else {
    ////        TileManager tile = tiles[CursorX, CursorY];

    ////        if (tile != null) {
    ////            // TODO: Play "can't do that" sound
    ////        } else {
    ////            // TODO: Play "dropped" sound
    ////            // TODO: Place tile
    ////        }
    ////    }
    ////}

    ////private void HandleCancel() {
    ////    // TODO: Nothing here yet.
    ////}
}
