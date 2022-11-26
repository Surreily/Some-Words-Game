using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private static readonly Vector3 TileCharacterPosition = new Vector3(1f / 16f * 4.5f, 1f / 16f * 1.5f, 9f);
    private static readonly Vector3 TileBackgroundPosition = new Vector3(0f, 0f, 10f);

    private GameObject cursorGameObject;
    private GameObject border;

    private MovableBehaviour cursorMovableBehaviour;

    private Stack<IAction> actions;

    private CameraMovement cameraMovement;

    public MaterialStore MaterialStore { get; set; }

    private Level level;

    public void LoadBoard(JsonLevel jsonLevel) {
        actions = new Stack<IAction>();

        level = new Level(jsonLevel.Width, jsonLevel.Height, jsonLevel.CursorX, jsonLevel.CursorY);

        foreach (JsonTile jsonTile in jsonLevel.Tiles) {
            AddTile(jsonTile.Character[0], jsonTile.X, jsonTile.Y);
        }

        SetUpBorder();
        SetUpCursor(level.CursorX, level.CursorY);

        SetUpCameraMovement();
    }

    private void SetUpBorder() {
        border = new GameObject();
        border.transform.SetParent(transform, false);

        BorderRenderer borderRenderer = border.AddComponent<BorderRenderer>();
        borderRenderer.Z = 1;
        borderRenderer.Width = level.Width;
        borderRenderer.Height = level.Height;
        borderRenderer.Border = 2;
        borderRenderer.Material = MaterialStore.BorderMaterial;
    }

    private void SetUpCursor(int x, int y) {
        cursorGameObject = new GameObject();
        cursorGameObject.transform.SetParent(transform, false);
        cursorGameObject.transform.position = new Vector3(x, (level.Height - 1) - y, 0f);

        cursorMovableBehaviour = cursorGameObject.AddComponent<MovableBehaviour>();
        cursorMovableBehaviour.speed = 15f;
        cursorMovableBehaviour.distance = 1f;

        TextureRenderer textureRenderer = cursorGameObject.AddComponent<TextureRenderer>();
        textureRenderer.material = MaterialStore.CursorMaterial;
        textureRenderer.width = 1.05f;
        textureRenderer.height = 1.05f;
        textureRenderer.z = 5f;
    }

    private void SetUpCameraMovement() {
        cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        cameraMovement.Target(cursorGameObject);
    }

    private void AddTile(char character, int x, int y) {
        GameObject tileGameObject = new GameObject();

        tileGameObject.transform.SetParent(transform, false);
        tileGameObject.transform.position = new Vector3(x, (level.Height - 1) - y, 0f);

        MovableBehaviour movableBehaviour = tileGameObject.AddComponent<MovableBehaviour>();
        movableBehaviour.speed = 15f;
        movableBehaviour.distance = 1f;

        TileBackgroundRenderer characterRenderer = tileGameObject.AddComponent<TileBackgroundRenderer>();
        characterRenderer.Position = TileCharacterPosition;
        characterRenderer.Width = 1f;
        characterRenderer.Height = 1f;
        characterRenderer.Material = MaterialStore.GetRainbowFontMaterial(character);

        TileBackgroundRenderer backgroundRenderer = tileGameObject.AddComponent<TileBackgroundRenderer>();
        backgroundRenderer.Position = TileBackgroundPosition;
        backgroundRenderer.Width = 0.95f;
        backgroundRenderer.Height = 0.95f;
        backgroundRenderer.Material = MaterialStore.ImmovableItemBackgroundMaterial;

        level.Tiles[x, y] = new Tile {
            MovableBehaviour = movableBehaviour,
            CharacterRenderer = characterRenderer,
            BackgroundRenderer = backgroundRenderer,
            X = x,
            Y = y,
            Character = character,
        };
    }

    #region Action Stack

    private void DoAction(IAction action) {
        action.Do();
        actions.Push(action);
    }

    private void UndoAction() {
        if (actions.TryPop(out IAction action)) {
            action.Undo();
        }
    }

    #endregion

    public void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            HandleMovement(Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            HandleMovement(Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            HandleMovement(Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            HandleMovement(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            HandleInteract();
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            UndoAction();
        }
    }

    #region Handle Interact

    private void HandleInteract() {
        switch (level.CursorState) {
            case CursorState.Normal:
                HandleNormalInteract();
                break;
            case CursorState.Selected:
                HandleSelectedInteract();
                break;
            default:
                throw new InvalidOperationException("Unsupported cursor state.");
        }
    }

    private void HandleNormalInteract() {
        ITile tile = level.Tiles[level.CursorX, level.CursorY];

        if (tile == null) {
            // Play "error" sound.
            return;
        }

        CombinedAction combinedAction = new CombinedAction(
            new PickUpTileAction(level));

        DoAction(combinedAction);
    }

    private void HandleSelectedInteract() {
        CombinedAction combinedAction = new CombinedAction(
            new DropTileAction(level));

        DoAction(combinedAction);
    }

    #endregion

    #region Handle Movement

    private void HandleMovement(Direction direction) {
        switch (level.CursorState) {
            case CursorState.Normal:
                HandleNormalMovement(direction);
                break;
            case CursorState.Selected:
                HandleSelectedMovement(direction);
                break;
            default:
                throw new InvalidOperationException("Unsupported cursor state.");
        }
    }

    private void HandleNormalMovement(Direction direction) {
        int newX = level.CursorX + direction.GetXOffset();
        int newY = level.CursorY + direction.GetYOffset();

        if (newX < 0 || newX >= level.Width || newY < 0 || newY >= level.Height) {
            // TODO: Play "error" sound.
            return;
        }

        CombinedAction action = new CombinedAction(
            new UpdateCursorPositionAction(level, direction),
            new MoveObjectAction(cursorGameObject.GetComponent<MovableBehaviour>(), direction));

        DoAction(action);
    }

    private void HandleSelectedMovement(Direction direction) {
        int x = level.CursorX;
        int y = level.CursorY;

        List<ITile> tiles = new List<ITile>() {
            level.Tiles[x, y],
        };

        while (true) {
            x += direction.GetXOffset();
            y += direction.GetYOffset();

            if (!level.IsPositionOnBoard(x, y)) {
                // TODO: Play "error" sound.
                return;
            }

            ITile tile = level.Tiles[x, y];

            if (tile == null) {
                break;
            }

            tiles.Add(tile);
        }

        CombinedAction combinedAction = new CombinedAction();

        combinedAction.AddAction(new UpdateCursorPositionAction(level, direction));
        combinedAction.AddAction(new MoveObjectAction(cursorGameObject.GetComponent<MovableBehaviour>(), direction));

        for (int i = tiles.Count - 1; i >= 0; i--) {
            ITile tile = tiles[i];

            combinedAction.AddAction(new UpdateTilePositionAction(level, tile.X, tile.Y, direction));
            combinedAction.AddAction(new MoveObjectAction(tile.MovableBehaviour, direction));
        }

        DoAction(combinedAction);
    }

    #endregion

}
