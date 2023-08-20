using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapManager : MonoBehaviour {
        private MapState state;

        private Dictionary<(int, int), MapPathTileManager> pathTileManagerDictionary;
        private Dictionary<(int, int), MapLevelTileManager> levelTileManagerDictionary;

        private GameObject cursorObject;
        private int cursorX;
        private int cursorY;
        private Vector3 cursorTarget;

        public IGameManager GameManager { get; set; }

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
                if (TryGetLevel(cursorX, cursorY, out MapLevelTileManager levelTileManager)) {
                    MapUi.SetLevelTitleText(levelTileManager.Level.Title);
                }
                
                state = MapState.Ready;
            }
        }

        #endregion

        #region Load Map

        public void OpenMap(GameModel game) {
            SetUpPaths(game);
            SetUpLevels(game);
            SetUpCursor(game);
            SetPathTileTypes();
        }

        private void SetUpPaths(GameModel game) {
            pathTileManagerDictionary = new Dictionary<(int, int), MapPathTileManager>();

            foreach (PathModel path in game.Paths) {
                for (int x = 0; x < path.Width; x++) {
                    for (int y = 0; y < path.Height; y++) {
                        GameObject pathObject = new GameObject("Path Tile");
                        pathObject.transform.SetParent(transform, false);
                        pathObject.transform.Translate(path.X + x, path.Y + y, Layers.MapPath, Space.Self);

                        // TODO: Precalculate Type.
                        MapPathTileManager pathManager = pathObject.AddComponent<MapPathTileManager>();
                        pathManager.MaterialStore = GameManager.MaterialStore;
                        pathManager.X = path.X + x;
                        pathManager.Y = path.Y + y;
                        pathManager.IsOpen = true;

                        pathTileManagerDictionary.Add((path.X + x, path.Y + y), pathManager);
                    }
                }
            }
        }

        private void SetUpLevels(GameModel game) {
            levelTileManagerDictionary = new Dictionary<(int, int), MapLevelTileManager>();

            foreach (LevelModel level in game.Levels) {
                GameObject levelObject = new GameObject();
                levelObject.transform.SetParent(transform, false);
                levelObject.transform.Translate(level.X, level.Y, Layers.MapLevel, Space.Self);

                MapLevelTileManager levelManager = levelObject.AddComponent<MapLevelTileManager>();
                levelManager.MaterialStore = GameManager.MaterialStore;
                levelManager.Level = level;
                levelManager.X = level.X;
                levelManager.Y = level.Y;
                levelManager.IsOpen = true;

                levelTileManagerDictionary.Add((level.X, level.Y), levelManager);
            }
        }

        private void SetUpCursor(GameModel game) {
            cursorObject = new GameObject();
            cursorObject.transform.SetParent(transform, false);
            cursorObject.transform.Translate(game.StartX, game.StartY, Layers.MapCursor, Space.Self);

            TileRenderer tileRenderer = cursorObject.AddComponent<TileRenderer>();
            tileRenderer.Material = GameManager.MaterialStore.Ui.GetCursorMaterial();
            tileRenderer.Size = 2f;

            GameManager.CameraMovement.Target(cursorObject);
        }

        private void SetPathTileTypes() {
            foreach (KeyValuePair<(int, int), MapPathTileManager> pair in pathTileManagerDictionary) {
                int x = pair.Key.Item1;
                int y = pair.Key.Item2;
                MapPathTileManager tile = pair.Value;

                bool up = DoesPathExist(x, y, Direction.Up);
                bool right = DoesPathExist(x, y, Direction.Right);
                bool down = DoesPathExist(x, y, Direction.Down);
                bool left = DoesPathExist(x, y, Direction.Left);

                tile.Type = PathTileTypeHelper.GetPathTileType(up, right, down, left);
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
            if (TryGetLevel(cursorX, cursorY, out MapLevelTileManager tile)) {
                GameManager.OpenLevel(tile.Level);
            }
        }

        #endregion

        #region Helpers

        private bool DoesPathExist(int x, int y) {
            return pathTileManagerDictionary.ContainsKey((x, y)) || levelTileManagerDictionary.ContainsKey((x, y));
        }

        private bool DoesPathExist(int x, int y, Direction direction) {
            return DoesPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool DoesOpenPathExist(int x, int y) {
            if (pathTileManagerDictionary.TryGetValue((x, y), out MapPathTileManager path)) {
                return path.IsOpen;
            }

            if (levelTileManagerDictionary.TryGetValue((x, y), out MapLevelTileManager level)) {
                return level.IsOpen;
            }

            return false;
        }

        private bool DoesOpenPathExist(int x, int y, Direction direction) {
            return DoesOpenPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool TryGetLevel(int x, int y, out MapLevelTileManager tile) {
            if (levelTileManagerDictionary.TryGetValue((x, y), out tile)) {
                return true;
            }

            return false;
        }

        #endregion

    }
}