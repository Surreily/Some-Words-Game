using System;
using System.Collections.Generic;
using System.Linq;
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

            EnterRevealingState(1, 3);
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
                case MapState.Revealing:
                    UpdateRevealingState();
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

        #region Revealing State

        private float currentRevealSeconds;
        private float targetRevealSeconds;
        private List<MapPathTileManager> revealedPathManagers;
        private List<MapLevelTileManager> revealedLevelManagers;

        private void EnterRevealingState(int x, int y) {
            // Initialise values.
            currentRevealSeconds = 0f;
            targetRevealSeconds = 0.2f;
            revealedPathManagers = new List<MapPathTileManager>();
            revealedLevelManagers = new List<MapLevelTileManager>();

            if (TryGetPathManager(x, y, out MapPathTileManager pathManager)) {
                revealedPathManagers.Add(pathManager);
            } else if (TryGetLevelManager(x, y, out MapLevelTileManager levelManager)) {
                revealedLevelManagers.Add(levelManager);
            }

            state = MapState.Revealing;
        }

        private void UpdateRevealingState() {
            // Handle timing. Each iteration is slightly quicker than the last.
            currentRevealSeconds += Time.deltaTime;

            if (currentRevealSeconds < targetRevealSeconds) {
                return;
            }

            if (targetRevealSeconds > 0.05f) {
                targetRevealSeconds -= 0.02f;
            }

            currentRevealSeconds -= targetRevealSeconds;

            // Reveal paths and levels.
            List<MapPathTileManager> revealingPathManagers = new List<MapPathTileManager>();
            List<MapLevelTileManager> revealingLevelManagers = new List<MapLevelTileManager>();

            HashSet<(int, int)> revealingCoordinates = new HashSet<(int, int)>();

            foreach (MapPathTileManager revealedPathManager in revealedPathManagers) {
                // Get the next generation of path managers.
                IEnumerable<MapPathTileManager> adjacentPathManagers = GetAdjacentPathManagers(
                    revealedPathManager.X, revealedPathManager.Y);

                foreach (MapPathTileManager adjacentPathManager in adjacentPathManagers) {
                    if (adjacentPathManager.State != PathState.Closed) {
                        continue;
                    }

                    if (revealingCoordinates.Contains((adjacentPathManager.X, adjacentPathManager.Y))) {
                        continue;
                    }

                    revealingPathManagers.Add(adjacentPathManager);
                    revealingCoordinates.Add((adjacentPathManager.X, adjacentPathManager.Y));
                }

                // Get the next generation of level managers.
                IEnumerable<MapLevelTileManager> adjacentLevelManagers = GetAdjacentLevelManagers(
                    revealedPathManager.X, revealedPathManager.Y);

                foreach (MapLevelTileManager adjacentLevelManager in adjacentLevelManagers) {
                    if (adjacentLevelManager.State != LevelState.Closed) {
                        continue;
                    }

                    if (revealingCoordinates.Contains((adjacentLevelManager.X, adjacentLevelManager.Y))) {
                        continue;
                    }

                    revealingCoordinates.Add((adjacentLevelManager.X, adjacentLevelManager.Y));
                }
            }

            // Update path managers.
            foreach (MapPathTileManager revealedPathManager in revealedPathManagers) {
                revealedPathManager.State = PathState.Open;
                revealedPathManager.Pulse();

                // TODO: Re-render the sprite.
            }

            // Update level managers.
            foreach (MapLevelTileManager revealedLevelManager in revealedLevelManagers) {
                revealedLevelManager.State = LevelState.Open;

                // TODO: Fancy animation for level opening.
                // TODO: Re-render the sprite.
            }

            // Exit the revealing state if we're done.
            if (!revealedPathManagers.Any() && !revealedLevelManagers.Any()) {
                state = MapState.Ready;
                return;
            }

            // Prepare for next iteration.
            revealedPathManagers = revealingPathManagers;
            revealedLevelManagers = revealingLevelManagers;
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

        public void AddPathManager(PathModel path, PathTileType type, PathState state) {
            GameObject pathObject = new GameObject("Path Tile");
            pathObject.transform.SetParent(gameObject.transform, false);
            pathObject.transform.Translate(path.X, path.Y, Layers.MapPath, Space.Self);

            MapPathTileManager pathManager = pathObject.AddComponent<MapPathTileManager>();
            pathManager.MaterialStore = GameManager.MaterialStore;
            pathManager.X = path.X;
            pathManager.Y = path.Y;
            pathManager.Type = type;
            pathManager.State = state;
            pathManager.Color = path.Color;

            pathManagers.Add((path.X, path.Y), pathManager);
        }

        public void AddLevelManager(LevelModel level, LevelState state) {
            GameObject levelObject = new GameObject();
            levelObject.transform.SetParent(gameObject.transform, false);
            levelObject.transform.Translate(level.X, level.Y, Layers.MapLevel, Space.Self);

            MapLevelTileManager levelManager = levelObject.AddComponent<MapLevelTileManager>();
            levelManager.MaterialStore = GameManager.MaterialStore;
            levelManager.Level = level;
            levelManager.X = level.X;
            levelManager.Y = level.Y;
            levelManager.State = state;
            levelManager.Color = level.Color;

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
                return path.State == PathState.Open;
            }

            if (levelManagers.TryGetValue((x, y), out MapLevelTileManager level)) {
                return level.State == LevelState.Open || level.State == LevelState.Complete;
            }

            return false;
        }

        private bool DoesOpenPathExist(int x, int y, Direction direction) {
            return DoesOpenPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool TryGetPathManager(int x, int y, out MapPathTileManager pathManager) {
            if (pathManagers.TryGetValue((x, y), out pathManager)) {
                return true;
            }

            return false;
        }

        private bool TryGetPathManager(int x, int y, Direction direction, out MapPathTileManager pathManager) {
            return TryGetPathManager(x + direction.GetXOffset(), y + direction.GetYOffset(), out pathManager);
        }

        private bool TryGetLevelManager(int x, int y, out MapLevelTileManager levelManager) {
            if (levelManagers.TryGetValue((x, y), out levelManager)) {
                return true;
            }

            return false;
        }

        private bool TryGetLevelManager(int x, int y, Direction direction, out MapLevelTileManager levelManager) {
            return TryGetLevelManager(x + direction.GetXOffset(), y + direction.GetYOffset(), out levelManager);
        }

        private IEnumerable<MapPathTileManager> GetAdjacentPathManagers(int x, int y) {
            List<MapPathTileManager> pathManagers = new List<MapPathTileManager>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)).Cast<Direction>()) {
                if (TryGetPathManager(x, y, direction, out MapPathTileManager pathManager)) {
                    pathManagers.Add(pathManager);
                }
            }

            return pathManagers;
        }

        private IEnumerable<MapLevelTileManager> GetAdjacentLevelManagers(int x, int y) {
            List<MapLevelTileManager> levelManagers = new List<MapLevelTileManager>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)).Cast<Direction>()) {
                if (TryGetLevelManager(x, y, direction, out MapLevelTileManager levelManager)) {
                    levelManagers.Add(levelManager);
                }
            }

            return levelManagers;
        }

        #endregion

    }
}