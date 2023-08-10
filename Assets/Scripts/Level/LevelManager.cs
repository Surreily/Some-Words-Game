using System;
using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Level.Actions;
using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using TMPro;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Level {

    public class LevelManager : MonoBehaviour {
        private GameObject cursorGameObject;
        private CursorManager cursorManager;

        private GameObject bordersObject;
        private LevelModel level;

        private MovableBehaviour cursorMovableBehaviour;

        private Stack<IAction> actions;

        public IGameManager GameManager { get; set; }

        public LevelState State { get; set; }

        public TileManager[,] TileManagers { get; set; }

        public void OpenLevel(LevelModel level) {
            this.level = level;

            actions = new Stack<IAction>();

            TileManagers = new TileManager[level.Width, level.Height];

            foreach (LevelTileModel levelTile in level.Tiles) {
                AddTile(levelTile.Character, levelTile.X, levelTile.Y);
            }

            SetUpBorder();
            SetUpCursor(level.StartX, level.StartY);

            SetUpCameraMovement();
        }

        public void CloseLevel() {
            level = null;
            actions = null;
            TileManagers = null;

            Destroy(bordersObject);
        }

        #region Border

        private void SetUpBorder() {
            bordersObject = new GameObject("Borders");
            bordersObject.transform.parent = transform;

            CreateBorderTileAreaRenderer(0, level.Height, level.Width, 1, SquareTileSetPosition.Top);
            CreateBorderTileRenderer(level.Width, level.Height, SquareTileSetPosition.TopRight);
            CreateBorderTileAreaRenderer(level.Width, 0, 1, level.Height, SquareTileSetPosition.Right);
            CreateBorderTileRenderer(level.Width, -1, SquareTileSetPosition.BottomRight);
            CreateBorderTileAreaRenderer(0, -1, level.Width, 1, SquareTileSetPosition.Bottom);
            CreateBorderTileRenderer(-1, -1, SquareTileSetPosition.BottomLeft);
            CreateBorderTileAreaRenderer(-1, 0, 1, level.Height, SquareTileSetPosition.Left);
            CreateBorderTileRenderer(-1, level.Height, SquareTileSetPosition.TopLeft);
            CreateBorderTileAreaRenderer(0, 0, level.Width, level.Height, SquareTileSetPosition.Center);
        }

        private void CreateBorderTileRenderer(int x, int y, SquareTileSetPosition position) {
            GameObject borderObject = new GameObject("Border");
            borderObject.transform.parent = bordersObject.transform;
            borderObject.transform.localPosition = new Vector3(x, y, 0.1f);

            TileRenderer renderer = borderObject.AddComponent<TileRenderer>();
            renderer.Material = GameManager.MaterialStore.Level.GetBackgroundMaterial(position);
        }

        private void CreateBorderTileAreaRenderer(int x, int y, int width, int height, SquareTileSetPosition position) {
            GameObject borderObject = new GameObject("Border");
            borderObject.transform.parent = bordersObject.transform;
            borderObject.transform.localPosition = new Vector3(x, y, 0.1f);

            TileAreaRenderer renderer = borderObject.AddComponent<TileAreaRenderer>();
            renderer.Width = width;
            renderer.Height = height;
            renderer.Material = GameManager.MaterialStore.Level.GetBackgroundMaterial(position);
        }

        #endregion

        #region Cursor

        private void SetUpCursor(int x, int y) {
            cursorGameObject = new GameObject("Cursor");
            cursorGameObject.transform.parent = transform;
            cursorGameObject.transform.localPosition = new Vector3(x, y, -3f);

            cursorManager = cursorGameObject.AddComponent<CursorManager>();
            cursorManager.MaterialStore = GameManager.MaterialStore;
            cursorManager.X = x;
            cursorManager.Y = y;
        }

        #endregion

        private void SetUpCameraMovement() {
            GameManager.CameraMovement.Target(cursorGameObject);
        }

        private void AddTile(char character, int x, int y) {
            GameObject tileObject = new GameObject("Tile");
            tileObject.transform.parent = transform;
            tileObject.transform.localPosition = new Vector3(x, y, -1f);

            TileManager tileManager = tileObject.AddComponent<TileManager>();
            tileManager.MaterialStore = GameManager.MaterialStore;
            tileManager.Character = character;
            tileManager.State = TileState.Normal;
            tileManager.X = x;
            tileManager.Y = y;

            TileManagers[x, y] = tileManager;
        }

        #region Action Stack

        private void DoAction(IAction action) {
            action.Do();
            actions.Push(action);
        }

        private void UndoAction() {
            if (actions.TryPop(out IAction action)) {
                action.Undo();
                UpdateTileStates();
            }
        }

        #endregion

        public void Update() {
            if (GameManager.State == GameState.Level) {
                HandleInput();
            }
        }

        private void HandleInput() {
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
            switch (State) {
                case LevelState.Normal:
                    HandleNormalInteract();
                    break;
                case LevelState.Selected:
                    HandleSelectedInteract();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported cursor state.");
            }
        }

        private void HandleNormalInteract() {
            TileManager tileManager = TileManagers[cursorManager.X, cursorManager.Y];

            if (tileManager == null) {
                // TODO: Play "error" sound.
                return;
            }

            CombinedAction combinedAction = new CombinedAction(
                new PickUpTileAction(this));

            DoAction(combinedAction);
        }

        private void HandleSelectedInteract() {
            CombinedAction combinedAction = new CombinedAction(
                new DropTileAction(this));

            DoAction(combinedAction);
        }

        #endregion

        #region Handle Movement

        private void HandleMovement(Direction direction) {
            switch (State) {
                case LevelState.Normal:
                    HandleNormalMovement(direction);
                    break;
                case LevelState.Selected:
                    HandleSelectedMovement(direction);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported cursor state.");
            }
        }

        private void HandleNormalMovement(Direction direction) {
            if (!AreCoordinatesInBounds(cursorManager.X, cursorManager.Y, direction)) {
                // TODO: Play "error" sound.
                return;
            }

            CombinedAction action = new CombinedAction(
                new UpdateCursorPositionAction(cursorManager, direction),
                new MoveCursorAction(cursorManager, direction));

            DoAction(action);
        }

        private void HandleSelectedMovement(Direction direction) {
            int x = cursorManager.X;
            int y = cursorManager.Y;

            List<TileManager> tileManagersToMove = new List<TileManager>() {
                TileManagers[x, y],
            };

            while (true) {
                x += direction.GetXOffset();
                y += direction.GetYOffset();

                if (!AreCoordinatesInBounds(x, y)) {
                    // TODO: Play "error" sound.
                    return;
                }

                TileManager tileManagerToMove = TileManagers[x, y];

                if (tileManagerToMove == null) {
                    break;
                }

                tileManagersToMove.Add(tileManagerToMove);
            }

            CombinedAction combinedAction = new CombinedAction();

            combinedAction.Add(
                new UpdateCursorPositionAction(cursorManager, direction),
                new MoveCursorAction(cursorManager, direction));

            for (int i = tileManagersToMove.Count - 1; i >= 0; i--) {
                TileManager tileManagerToMove = tileManagersToMove[i];

                combinedAction.Add(
                    new UpdateTilePositionAction(this, tileManagerToMove.X, tileManagerToMove.Y, direction),
                    new MoveTileAction(tileManagerToMove, direction));
            }

            DoAction(combinedAction);

            UpdateTileStates();
        }

        #endregion

        #region Update Tile States

        private void UpdateTileStates() {
            List<UpdateTileStateAction> updateTileStateActions = new List<UpdateTileStateAction>();

            TileState[,] newStates = new TileState[level.Width, level.Height];

            List<List<TileManager>> horizontalSpans = new List<List<TileManager>>();
            List<List<TileManager>> verticalSpans = new List<List<TileManager>>();

            List<TileManager> currentSpan = new List<TileManager>();

            // Create spans of tiles in both directions.
            for (int y = level.Height - 1; y >= 0; y--) {
                for (int x = 0; x < level.Width; x++) {
                    TileManager tileManager = TileManagers[x, y];

                    if (tileManager != null) {
                        currentSpan.Add(tileManager);
                        continue;
                    }

                    if (currentSpan.Count > 0) {
                        horizontalSpans.Add(currentSpan);
                        currentSpan = new List<TileManager>();
                    }
                }

                if (currentSpan.Count > 0) {
                    horizontalSpans.Add(currentSpan);
                    currentSpan = new List<TileManager>();
                }
            }

            // TODO: Confirm that currentSpan is always empty at this point!
            // TODO: If we just ended on a span in progress, do we save that span?

            for (int x = 0; x < level.Width; x++) {
                for (int y = level.Height - 1; y >= 0; y--) {
                    TileManager tileManager = TileManagers[x, y];

                    if (tileManager != null) {
                        currentSpan.Add(tileManager);
                        continue;
                    }

                    if (currentSpan.Count > 0) {
                        verticalSpans.Add(currentSpan);
                        currentSpan = new List<TileManager>();
                    }
                }

                if (currentSpan.Count > 0) {
                    verticalSpans.Add(currentSpan);
                    currentSpan = new List<TileManager>();
                }
            }

            // TODO: If we just ended on a span in progress, do we save that span?

            // Check for valid words in both directions.
            foreach (List<TileManager> span in horizontalSpans) {
                bool isValidWord =
                    span.Count >= 3 &&
                    GameManager.Dictionary.Contains(new string(span.Select(t => t.Character).ToArray()));

                foreach (TileManager tileManager in span) {
                    newStates[tileManager.X, tileManager.Y] = isValidWord
                        ? TileState.Valid
                        : TileState.Normal;
                }
            }

            foreach (List<TileManager> span in verticalSpans) {
                bool isValidWord =
                    span.Count >= 3 &&
                    GameManager.Dictionary.Contains(new string(span.Select(t => t.Character).ToArray()));

                if (!isValidWord) {
                    continue;
                }

                foreach (TileManager tileManager in span) {
                    newStates[tileManager.X, tileManager.Y] = TileState.Valid;
                }
            }

            // Set tiles.
            for (int x = 0; x < level.Width; x++) {
                for (int y = 0; y < level.Height; y++) {
                    TileManager tileManager = TileManagers[x, y];

                    if (tileManager == null) {
                        continue;
                    }

                    TileState newTileState = newStates[x, y];

                    if (tileManager.State != newTileState) {
                        tileManager.State = newTileState;

                        if (newTileState == TileState.Valid) {
                            tileManager.Pulse();
                        }
                    }
                }
            }
        }

        #endregion

        private bool AreCoordinatesInBounds(int x, int y) {
            return x >= 0 && x < level.Width && y >= 0 && y < level.Height;
        }

        private bool AreCoordinatesInBounds(int x, int y, Direction direction) {
            return AreCoordinatesInBounds(x + direction.GetXOffset(), y + direction.GetYOffset());
        }
    }
}