using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Json.Game;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Ui;
using Surreily.SomeWords.Scripts.Utility;
using TMPro;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapManager : MonoBehaviour {
        private MapState state;

        private Dictionary<(int, int), MapPathTile> pathTileDictionary;
        private Dictionary<(int, int), MapLevelTile> levelTileDictionary;

        private GameObject cursorObject;
        private int cursorX;
        private int cursorY;
        private Vector3 cursorTarget;

        public GameManager GameManager { get; set; }

        public MaterialStore MaterialStore { get; set; }

        public MapUi MapUi { get; set; }

        #region Start

        public void Start() {
            state = MapState.Ready;
        }

        #endregion

        #region Update

        public void Update() {
            switch (state) {
                case MapState.Ready:
                    HandleInput();
                    break;
                case MapState.CursorMoving:
                    UpdateCursorMovingState();
                    break;
            }
        }

        private void UpdateCursorMovingState() {
            cursorObject.transform.localPosition = Vector3.MoveTowards(
                cursorObject.transform.localPosition, cursorTarget, Time.deltaTime * 10f);

            if (cursorObject.transform.localPosition == cursorTarget) {
                if (TryGetLevel(cursorX, cursorY, out MapLevelTile tile)) {
                    MapUi.SetLevelTitleText(tile.JsonLevel.Title);
                }
                
                state = MapState.Ready;
            }
        }

        #endregion

        #region Load Map

        public void LoadMap(JsonGame game) {
            SetUpPathObjects(game);
            SetUpLevelObjects(game);
            SetUpCursor(game);
            SetUpPathTileRenderers();
            SetUpLevelTileRenderers();
        }

        private void SetUpPathObjects(JsonGame game) {
            pathTileDictionary = new Dictionary<(int, int), MapPathTile>();

            foreach (JsonPath path in game.Paths) {
                for (int x = 0; x < path.Width; x++) {
                    for (int y = 0; y < path.Height; y++) {
                        GameObject pathObject = new GameObject("Path Tile");
                        pathObject.transform.SetParent(transform, false);
                        pathObject.transform.Translate(path.X + x, path.Y + y, Layers.MapPath, Space.Self);

                        MapPathTile pathTile = new MapPathTile {
                            GameObject = pathObject,
                            IsOpen = true,
                        };

                        pathTileDictionary.Add((path.X + x, path.Y + y), pathTile);
                    }
                }
            }
        }

        private void SetUpLevelObjects(JsonGame game) {
            levelTileDictionary = new Dictionary<(int, int), MapLevelTile>();

            foreach (JsonLevel level in game.Levels) {
                GameObject levelObject = new GameObject();
                levelObject.transform.SetParent(transform, false);
                levelObject.transform.Translate(level.X, level.Y, Layers.MapLevel, Space.Self);

                MapLevelTile levelTile = new MapLevelTile {
                    JsonLevel = level,
                    GameObject = levelObject,
                    IsOpen = true,
                };

                levelTileDictionary.Add((level.X, level.Y), levelTile);
            }
        }

        private void SetUpCursor(JsonGame game) {
            cursorObject = new GameObject();
            cursorObject.transform.SetParent(transform, false);
            cursorObject.transform.Translate(game.StartX, game.StartY, Layers.MapCursor, Space.Self);

            TileRenderer tileRenderer = cursorObject.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.Ui.GetCursorMaterial();
            tileRenderer.Size = 2f;

            GameManager.CameraMovement.Target(cursorObject);
        }

        private void SetUpPathTileRenderers() {
            foreach (KeyValuePair<(int, int), MapPathTile> pair in pathTileDictionary) {
                int x = pair.Key.Item1;
                int y = pair.Key.Item2;
                MapPathTile tile = pair.Value;

                bool up = DoesPathExist(x, y, Direction.Up);
                bool right = DoesPathExist(x, y, Direction.Right);
                bool down = DoesPathExist(x, y, Direction.Down);
                bool left = DoesPathExist(x, y, Direction.Left);

                PathTileType pathTileType = PathTileTypeHelper.GetPathTileType(up, right, down, left);

                TileRenderer tileRenderer = tile.GameObject.AddComponent<TileRenderer>();

                if (tile.IsOpen) {
                    tileRenderer.Material = MaterialStore.Map.GetOpenPathMaterial(tile.Colour, pathTileType);
                } else {
                    tileRenderer.Material = MaterialStore.Map.GetClosedPathMaterial(pathTileType);
                }
            }
        }

        private void SetUpLevelTileRenderers() {
            foreach (KeyValuePair<(int, int), MapLevelTile> pair in levelTileDictionary) {
                int x = pair.Key.Item1;
                int y = pair.Key.Item2;
                MapLevelTile tile = pair.Value;

                TileRenderer tileRenderer = tile.GameObject.AddComponent<TileRenderer>();

                if (tile.IsOpen) {
                    tileRenderer.Material = MaterialStore.Map.GetOpenLevelMaterial(tile.Colour);
                } else {
                    tileRenderer.Material = MaterialStore.Map.GetClearedLevelMaterial(tile.Colour);
                }
            }
        }

        #endregion

        #region Input

        private void HandleInput() {
            if (GameManager.State != GameState.Map) {
                return;
            }

            bool up = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            bool right = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
            bool down = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
            bool left = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);

            if (up && !right && !down && !left) {
                HandleMove(Direction.Up);
                return;
            }
            
            if (right && !up && !down && !left) {
                HandleMove(Direction.Right);
                return;
            }
            
            if (down && !up && !right && !left) {
                HandleMove(Direction.Down);
                return;
            }
            
            if (left && !up && !right && !down) {
                HandleMove(Direction.Left);
                return;
            }

            bool enter = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return);

            if (enter) {
                HandleEnter();
                return;
            }
        }

        private void HandleMove(Direction direction) {
            int newX = cursorX;
            int newY = cursorY;

            while (DoesOpenPathExist(newX, newY, direction)) {
                newX += direction.GetXOffset();
                newY += direction.GetYOffset();

                if (DoesOpenPathExist(newX, newY, direction.GetNextClockwise()) ||
                    DoesOpenPathExist(newX, newY, direction.GetNextAnticlockwise())) {
                    break;
                }
            }

            if (newX == cursorX && newY == cursorY) {
                return;
            }

            cursorX = newX;
            cursorY = newY;
            cursorTarget = new Vector3(cursorX, cursorY, 0f);

            MapUi.ClearLevelTitleText();

            state = MapState.CursorMoving;
        }

        private void HandleEnter() {
            if (TryGetLevel(cursorX, cursorY, out MapLevelTile tile)) {
                GameManager.OpenLevel(tile.JsonLevel);
            }
        }

        #endregion

        private bool DoesPathExist(int x, int y) {
            return pathTileDictionary.ContainsKey((x, y)) || levelTileDictionary.ContainsKey((x, y));
        }

        private bool DoesPathExist(int x, int y, Direction direction) {
            return DoesPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool DoesOpenPathExist(int x, int y) {
            if (pathTileDictionary.TryGetValue((x, y), out MapPathTile path)) {
                return path.IsOpen;
            }

            if (levelTileDictionary.TryGetValue((x, y), out MapLevelTile level)) {
                return level.IsOpen;
            }

            return false;
        }

        private bool DoesOpenPathExist(int x, int y, Direction direction) {
            return DoesOpenPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool TryGetLevel(int x, int y, out MapLevelTile tile) {
            if (levelTileDictionary.TryGetValue((x, y), out tile)) {
                return true;
            }

            return false;
        }

        private class MapPathTile {
            public GameObject GameObject { get; set; }
            public int Colour { get; set; }
            public bool IsOpen { get; set; }
        }

        private class MapLevelTile {
            public GameObject GameObject { get; set; }
            public JsonLevel JsonLevel { get; set; }
            public int Colour { get; set; }
            public bool IsOpen { get; set; }
        }
    }
}