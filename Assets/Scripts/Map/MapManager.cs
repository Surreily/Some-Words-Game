using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapManager : MonoBehaviour {
        private MapState state;

        private Dictionary<(int, int), MapPathTileManager> pathManagers;
        private Dictionary<(int, int), MapLevelTileManager> levelManagers;

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
                if (TryGetLevelManager(cursorX, cursorY, out MapLevelTileManager levelTileManager)) {
                    MapUi.SetLevelTitleText(levelTileManager.Level.Title);
                }
                
                state = MapState.Ready;
            }
        }

        #endregion

        #region Load Map

        public void OpenMap(GameModel game) {
            pathManagers = new Dictionary<(int, int), MapPathTileManager>();
            levelManagers = new Dictionary<(int, int), MapLevelTileManager>();

            MapLoader mapLoader = new MapLoader(this, game);
            mapLoader.Load();

            SetUpCursor(game);
        }

        public void AddPathManager(PathModel path, int x, int y, PathTileType type, MapPathState state) {
            GameObject pathObject = new GameObject("Path Tile");
            pathObject.transform.SetParent(gameObject.transform, false);
            pathObject.transform.Translate(x, y, Layers.MapPath, Space.Self);

            MapPathTileManager pathManager = pathObject.AddComponent<MapPathTileManager>();
            pathManager.MaterialStore = GameManager.MaterialStore;
            pathManager.X = x;
            pathManager.Y = y;
            pathManager.Type = type;
            pathManager.State = state;

            pathManagers.Add((x, y), pathManager);
        }

        public void AddLevelManager(LevelModel level, MapLevelState state) {
            GameObject levelObject = new GameObject();
            levelObject.transform.SetParent(gameObject.transform, false);
            levelObject.transform.Translate(level.X, level.Y, Layers.MapLevel, Space.Self);

            MapLevelTileManager levelManager = levelObject.AddComponent<MapLevelTileManager>();
            levelManager.MaterialStore = GameManager.MaterialStore;
            levelManager.Level = level;
            levelManager.X = level.X;
            levelManager.Y = level.Y;
            levelManager.State = state;

            levelManagers.Add((level.X, level.Y), levelManager);
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
            if (TryGetLevelManager(cursorX, cursorY, out MapLevelTileManager tile)) {
                GameManager.OpenLevel(tile.Level);
            }
        }

        #endregion

        #region Helpers

        private bool DoesPathExist(int x, int y) {
            return pathManagers.ContainsKey((x, y)) || levelManagers.ContainsKey((x, y));
        }

        private bool DoesPathExist(int x, int y, Direction direction) {
            return DoesPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool DoesOpenPathExist(int x, int y) {
            if (pathManagers.TryGetValue((x, y), out MapPathTileManager path)) {
                return path.State == MapPathState.Open;
            }

            if (levelManagers.TryGetValue((x, y), out MapLevelTileManager level)) {
                return level.State == MapLevelState.Open || level.State == MapLevelState.Complete;
            }

            return false;
        }

        private bool DoesOpenPathExist(int x, int y, Direction direction) {
            return DoesOpenPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool TryGetLevelManager(int x, int y, out MapLevelTileManager levelManager) {
            if (levelManagers.TryGetValue((x, y), out levelManager)) {
                return true;
            }

            return false;
        }

        #endregion

    }
}