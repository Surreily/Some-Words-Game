using System;
using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Level.Actions;
using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Level {

    public class LevelManager : MonoBehaviour {
        private GameObject levelObject;

        private CursorManager cursorManager;

        private GameObject bordersObject;
        private LevelModel level;

        private Stack<IAction> actions;

        public IGameManager GameManager { get; set; }

        public LevelState State { get; set; }

        public TileManager[,] TileManagers { get; set; }

        public void OpenLevel(LevelModel level) {
            

            this.level = level;
            actions = new Stack<IAction>();

            CreateLevelObject();
            CreateTiles(level);
            CreateBorder();
            CreateCursor(level.StartX, level.StartY);

            SetUpCameraMovement();
        }

        public void CloseLevel() {
            level = null;
            actions = null;

            DestroyBorder();
            DestroyCursor();
            DestroyTiles();
            DestroyLevelObject();
        }

        #region Level Object

        private void CreateLevelObject() {
            levelObject = new GameObject("Level");
            levelObject.transform.parent = transform;
            levelObject.transform.localPosition = new Vector3(level.X - level.StartX, level.Y - level.StartY, 0f);
        }

        private void DestroyLevelObject() {
            Destroy(levelObject);
        }

        #endregion

        #region Border

        private void CreateBorder() {
            bordersObject = new GameObject("Borders");
            bordersObject.transform.parent = levelObject.transform;
            bordersObject.transform.localPosition = Vector3.zero;

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

        private void DestroyBorder() {
            Destroy(bordersObject);
            bordersObject = null;
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

        private void CreateCursor(int x, int y) {
            GameObject cursorGameObject = new GameObject("Cursor");
            cursorGameObject.transform.parent = levelObject.transform;
            cursorGameObject.transform.localPosition = new Vector3(x, y, -3f);

            cursorManager = cursorGameObject.AddComponent<CursorManager>();
            cursorManager.MaterialStore = GameManager.MaterialStore;
            cursorManager.X = x;
            cursorManager.Y = y;
        }

        private void DestroyCursor() {
            Destroy(cursorManager.gameObject);
            cursorManager = null;
        }

        #endregion

        private void SetUpCameraMovement() {
            GameManager.CameraMovement.Target(cursorManager.gameObject);
        }

        #region Tiles

        private void CreateTiles(LevelModel level) {
            TileManagers = new TileManager[level.Width, level.Height];

            foreach (LevelTileModel levelTile in level.Tiles) {
                CreateTile(levelTile.Character, levelTile.X, levelTile.Y);
            }
        }

        private void CreateTile(char character, int x, int y) {
            GameObject tileObject = new GameObject("Tile");
            tileObject.transform.parent = levelObject.transform;
            tileObject.transform.localPosition = new Vector3(x, y, -1f);

            TileManager tileManager = tileObject.AddComponent<TileManager>();
            tileManager.MaterialStore = GameManager.MaterialStore;
            tileManager.Character = character;
            tileManager.State = TileState.Normal;
            tileManager.X = x;
            tileManager.Y = y;

            TileManagers[x, y] = tileManager;
        }

        private void DestroyTiles() {
            foreach (TileManager tileManager in TileManagers) {
                if (tileManager != null) {
                    Destroy(tileManager.gameObject);
                }
            }

            TileManagers = null;
        }

        #endregion

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
            HandleInput();
        }

        #region Input

        private void HandleInput() {
            if (GameManager.State != GameState.Level) {
                return;
            }

            bool enter = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return);

            if (enter) {
                TryInteractWithTile();
                return;
            }

            bool backspace = Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Z);

            if (backspace) {
                UndoAction();
                return;
            }

            bool up = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            bool right = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
            bool down = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
            bool left = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);

            if (up && !right && !down && !left) {
                TryMove(Direction.Up);
                return;
            }

            if (right && !up && !down && !left) {
                TryMove(Direction.Right);
                return;
            }

            if (down && !up && !right && !left) {
                TryMove(Direction.Down);
                return;
            }

            if (left && !up && !right && !down) {
                TryMove(Direction.Left);
                return;
            }

            bool escape = Input.GetKeyDown(KeyCode.Escape);

            if (escape) {
                GameManager.CloseLevel();
            }
        }

        #endregion

        #region Interact

        private void TryInteractWithTile() {
            switch (State) {
                case LevelState.Normal:
                    TryPickUpTile();
                    break;
                case LevelState.Selected:
                    TryDropTile();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported cursor state.");
            }
        }

        private void TryPickUpTile() {
            TileManager tileManager = TileManagers[cursorManager.X, cursorManager.Y];

            if (tileManager == null) {
                // TODO: Play "error" sound.
                return;
            }

            CombinedAction combinedAction = new CombinedAction(
                new PickUpTileAction(this));

            DoAction(combinedAction);
        }

        private void TryDropTile() {
            CombinedAction combinedAction = new CombinedAction(
                new DropTileAction(this));

            DoAction(combinedAction);
        }

        #endregion

        #region Move

        private void TryMove(Direction direction) {
            switch (State) {
                case LevelState.Normal:
                    TryMoveWithoutTile(direction);
                    break;
                case LevelState.Selected:
                    TryMoveWithTile(direction);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported cursor state.");
            }
        }

        private void TryMoveWithoutTile(Direction direction) {
            if (!AreCoordinatesInBounds(cursorManager.X, cursorManager.Y, direction)) {
                // TODO: Play "error" sound.
                return;
            }

            CombinedAction action = new CombinedAction(
                new UpdateCursorPositionAction(cursorManager, direction),
                new MoveCursorAction(cursorManager, direction));

            DoAction(action);
        }

        private void TryMoveWithTile(Direction direction) {
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

                        switch (newTileState) {
                            case TileState.Normal:
                                tileManager.SetFontColor(Color.white);
                                break;
                            case TileState.Valid:
                                tileManager.SetFontColor(Color.cyan);
                                tileManager.Pulse();
                                break;
                            case TileState.Invalid:
                                tileManager.SetFontColor(Color.red);
                                break;
                            default:
                                throw new InvalidOperationException($"TileState {newTileState} not recognised.");
                        }
                    }
                }
            }
        }

        #endregion

        #region Helpers

        private bool AreCoordinatesInBounds(int x, int y) {
            return x >= 0 && x < level.Width && y >= 0 && y < level.Height;
        }

        private bool AreCoordinatesInBounds(int x, int y, Direction direction) {
            return AreCoordinatesInBounds(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        #endregion

    }
}