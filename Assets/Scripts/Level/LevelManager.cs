using System;
using System.Collections.Generic;
using System.Linq;
using Surreily.SomeWords.Scripts.Json.Game;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using TMPro;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Level {

    public class LevelManager : MonoBehaviour {
        private static readonly Vector3 TileCharacterPosition = new Vector3(1f / 16f * 4.5f, 1f / 16f * 1.5f, 0f);
        private static readonly Vector3 TileBackgroundPosition = new Vector3(0f, 0f, 0f);

        private GameObject cursorGameObject;

        private MovableBehaviour cursorMovableBehaviour;

        private Stack<IAction> actions;

        private CameraMovement cameraMovement;

        public MaterialStore MaterialStore { get; set; }
        public HashSet<string> GameDictionary { get; set; }

        private Surreily.SomeWords.Scripts.Level.Level level;

        public void LoadBoard(JsonLevel jsonLevel) {
            actions = new Stack<IAction>();

            level = new Surreily.SomeWords.Scripts.Level.Level(jsonLevel.Width, jsonLevel.Height, jsonLevel.StartX, jsonLevel.StartY);

            foreach (JsonTile jsonTile in jsonLevel.Tiles) {
                AddTile(jsonTile.Character[0], jsonTile.X, jsonTile.Y);
            }

            SetUpBorder();

            SetUpCursor(level.CursorX, level.CursorY);

            level.CursorMovableBehaviour = cursorMovableBehaviour;

            SetUpCameraMovement();
        }

        private void SetUpBorder() {
            // TODO: Put all these under a single GameObject to clean up the hierarchy.
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
            GameObject child = new GameObject("Border");
            child.transform.parent = transform;
            child.transform.localPosition = new Vector3(x, y, 0.1f);

            TileRenderer renderer = child.AddComponent<TileRenderer>();
            renderer.Material = MaterialStore.Level.GetBackgroundMaterial(position);
        }

        private void CreateBorderTileAreaRenderer(int x, int y, int width, int height, SquareTileSetPosition position) {
            GameObject child = new GameObject("Border");
            child.transform.parent = transform;
            child.transform.localPosition = new Vector3(x, y, 0.1f);

            TileAreaRenderer renderer = child.AddComponent<TileAreaRenderer>();
            renderer.Width = width;
            renderer.Height = height;
            renderer.Material = MaterialStore.Level.GetBackgroundMaterial(position);
        }

        private void SetUpCursor(int x, int y) {
            cursorGameObject = new GameObject("Cursor");
            cursorGameObject.transform.parent = transform;
            cursorGameObject.transform.localPosition = new Vector3(x, y, -10f);

            cursorMovableBehaviour = cursorGameObject.AddComponent<MovableBehaviour>();
            cursorMovableBehaviour.speed = 15f;
            cursorMovableBehaviour.distance = 1f;

            TextureRenderer textureRenderer = cursorGameObject.AddComponent<TextureRenderer>();
            textureRenderer.material = MaterialStore.Ui.GetCursorMaterial();
            textureRenderer.width = 1.05f;
            textureRenderer.height = 1.05f;
            textureRenderer.z = 5f;
        }

        private void SetUpCameraMovement() {
            cameraMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
            cameraMovement.Target(cursorGameObject);
        }

        private void AddTile(char character, int x, int y) {
            GameObject tileObject = new GameObject("Tile");
            tileObject.transform.parent = transform;
            tileObject.transform.localPosition = new Vector3(x, y, -1f);

            MovableBehaviour movableBehaviour = tileObject.AddComponent<MovableBehaviour>();
            movableBehaviour.speed = 15f;
            movableBehaviour.distance = 1f;

            GameObject backgroundObject = new GameObject("Background");
            backgroundObject.transform.parent = tileObject.transform;
            backgroundObject.transform.localPosition = Vector3.zero;
            backgroundObject.transform.localScale = Vector3.one;

            SpriteRenderer backgroundRenderer = backgroundObject.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = MaterialStore.Level.DefaultTileSprite;

            GameObject characterObject = new GameObject();

            characterObject.transform.parent = tileObject.transform;
            characterObject.transform.localPosition = Vector3.zero;

            RectTransform rectTransform = characterObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            TMP_Text textMeshProText = characterObject.AddComponent<TextMeshPro>();
            textMeshProText.font = Resources.Load<TMP_FontAsset>("Fonts/VGA Font");
            textMeshProText.text = character.ToString();
            textMeshProText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            textMeshProText.verticalAlignment = VerticalAlignmentOptions.Middle;
            textMeshProText.fontSize = 8f;

            ////TileBackgroundRenderer characterRenderer = characterObject.AddComponent<TileBackgroundRenderer>();
            ////characterRenderer.Position = TileCharacterPosition;
            ////characterRenderer.Width = 1f;
            ////characterRenderer.Height = 1f;
            ////characterRenderer.Material = MaterialStore.Font.GetRainbowFontMaterial(character);

            PulseAnimationBehaviour pulseAnimationBehaviour = characterObject.AddComponent<PulseAnimationBehaviour>();
            pulseAnimationBehaviour.Scale = 2f;
            pulseAnimationBehaviour.Speed = 5f;

            Tile tile = new Tile(MaterialStore, movableBehaviour, pulseAnimationBehaviour, textMeshProText, backgroundRenderer) {
                X = x,
                Y = y,
                Character = character,
            };

            level.Tiles[x, y] = tile;
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
                new MoveCursorAction(level, direction));

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

            combinedAction.Add(
                new UpdateCursorPositionAction(level, direction),
                new MoveCursorAction(level, direction));

            for (int i = tiles.Count - 1; i >= 0; i--) {
                ITile tile = tiles[i];

                combinedAction.Add(
                    new UpdateTilePositionAction(level, tile.X, tile.Y, direction),
                    new MoveTileAction(tile, direction));
            }

            DoAction(combinedAction);

            UpdateTileStates();
        }

        #endregion

        #region Update Tile States

        private void UpdateTileStates() {
            List<UpdateTileStateAction> updateTileStateActions = new List<UpdateTileStateAction>();

            TileState[,] newStates = new TileState[level.Width, level.Height];

            List<List<ITile>> horizontalSpans = new List<List<ITile>>();
            List<List<ITile>> verticalSpans = new List<List<ITile>>();

            List<ITile> currentSpan = new List<ITile>();

            // Create spans of tiles in both directions.
            for (int y = 0; y < level.Height; y++) {
                for (int x = 0; x < level.Width; x++) {
                    ITile tile = level.Tiles[x, y];

                    if (tile != null) {
                        currentSpan.Add(tile);
                        continue;
                    }

                    if (currentSpan.Count > 0) {
                        horizontalSpans.Add(currentSpan);
                        currentSpan = new List<ITile>();
                    }
                }

                if (currentSpan.Count > 0) {
                    horizontalSpans.Add(currentSpan);
                    currentSpan = new List<ITile>();
                }
            }

            for (int x = 0; x < level.Width; x++) {
                for (int y = 0; y < level.Height; y++) {
                    ITile tile = level.Tiles[x, y];

                    if (tile != null) {
                        currentSpan.Add(tile);
                        continue;
                    }

                    if (currentSpan.Count > 0) {
                        verticalSpans.Add(currentSpan);
                        currentSpan = new List<ITile>();
                    }
                }

                if (currentSpan.Count > 0) {
                    verticalSpans.Add(currentSpan);
                    currentSpan = new List<ITile>();
                }
            }

            // Check for valid words in both directions.
            foreach (List<ITile> span in horizontalSpans) {
                bool isValidWord =
                    span.Count >= 3 &&
                    GameDictionary.Contains(new string(span.Select(t => t.Character).ToArray()));

                foreach (ITile tile in span) {
                    newStates[tile.X, tile.Y] = isValidWord
                        ? TileState.Valid
                        : TileState.Normal;
                }
            }

            foreach (List<ITile> span in verticalSpans) {
                bool isValidWord =
                    span.Count >= 3 &&
                    GameDictionary.Contains(new string(span.Select(t => t.Character).ToArray()));

                if (!isValidWord) {
                    continue;
                }

                foreach (ITile tile in span) {
                    newStates[tile.X, tile.Y] = TileState.Valid;
                }
            }

            // Set tiles.
            for (int x = 0; x < level.Width; x++) {
                for (int y = 0; y < level.Height; y++) {
                    ITile tile = level.Tiles[x, y];

                    if (tile == null) {
                        continue;
                    }

                    TileState newTileState = newStates[x, y];

                    if (tile.TileState != newTileState) {
                        tile.TileState = newTileState;

                        if (newTileState == TileState.Valid) {
                            tile.AnimatePulse();
                        }
                    }
                }
            }
        }

        #endregion

    }
}