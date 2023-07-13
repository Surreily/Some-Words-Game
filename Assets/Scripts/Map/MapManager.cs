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

        private TMP_Text levelTitleText;

        private Dictionary<(int, int), MapPathManager> pathDictionary;
        private Dictionary<(int, int), MapLevelManager> levelDictionary;

        private GameObject cursorObject;
        private int cursorX;
        private int cursorY;
        private Vector3 cursorTarget;

        public GameManager GameManager { get; set; }

        public MaterialStore MaterialStore { get; set; }

        #region Start

        public void Start() {
            levelTitleText = GameObject.Find("Map Canvas/Level Title").GetComponent<TMP_Text>();

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
                levelTitleText.text = "Moving stopped!"; // TODO: Get this from the map.
                state = MapState.Ready;
            }
        }

        #endregion

        #region Load Map

        public void LoadMap(JsonGame game) {
            SetUpPathDictionary(game);
            SetUpLevelDictionary(game);
            SetUpCursor();

            GameManager.CameraMovement.Target(cursorObject);

            CalculatePathTypes();
        }

        private void SetUpPathDictionary(JsonGame game) {
            pathDictionary = new Dictionary<(int, int), MapPathManager>();

            foreach (JsonPath path in game.Paths) {
                for (int x = 0; x < path.Width; x++) {
                    for (int y = 0; y < path.Height; y++) {
                        GameObject pathObject = new GameObject();
                        pathObject.transform.SetParent(transform, false);
                        pathObject.transform.Translate(path.X + x, path.Y + y, Layers.MapPath, Space.Self);

                        MapPathManager pathManager = pathObject.AddComponent<MapPathManager>();
                        pathManager.MaterialStore = MaterialStore;
                        pathManager.Colour = path.Colour;
                        pathManager.IsOpen = true; // TODO: Get from JSON or calculate.

                        pathDictionary.Add((path.X + x, path.Y + y), pathManager);
                    }
                }
            }
        }

        private void SetUpLevelDictionary(JsonGame game) {
            levelDictionary = new Dictionary<(int, int), MapLevelManager>();

            foreach (JsonLevel level in game.Levels) {
                GameObject levelObject = new GameObject();
                levelObject.transform.SetParent(transform, false);
                levelObject.transform.Translate(level.X, level.Y, Layers.MapLevel, Space.Self);

                MapLevelManager levelManager = levelObject.AddComponent<MapLevelManager>();
                levelManager.MaterialStore = MaterialStore;
                levelManager.Colour = level.Colour;
                levelManager.IsOpen = true; // TODO: Get from JSON or calculate.

                levelDictionary.Add((level.X, level.Y), levelManager);
            }
        }

        private void SetUpCursor() {
            cursorObject = new GameObject();
            cursorObject.transform.SetParent(transform, false);
            cursorObject.transform.Translate(0f, 0f, Layers.MapCursor, Space.Self); // TODO: Get this from the map.

            TileRenderer tileRenderer = cursorObject.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.Ui.GetCursorMaterial();
            tileRenderer.Size = 2f;
        }

        private void CalculatePathTypes() {
            foreach (KeyValuePair<(int, int), MapPathManager> item in pathDictionary) {
                int x = item.Key.Item1;
                int y = item.Key.Item2;
                MapPathManager pathManager = item.Value;

                bool up = DoesPathExist(x, y, Direction.Up);
                bool right = DoesPathExist(x, y, Direction.Right);
                bool down = DoesPathExist(x, y, Direction.Down);
                bool left = DoesPathExist(x, y, Direction.Left);

                pathManager.TileType = GetPathTileType(up, right, down, left);
            }
        }

        private PathTileType GetPathTileType(bool up, bool right, bool down, bool left) {
            PathTileType value = PathTileType.None;

            if (up) {
                value |= PathTileType.Up;
            }

            if (right) {
                value |= PathTileType.Right;
            }

            if (down) {
                value |= PathTileType.Down;
            }

            if (left) {
                value |= PathTileType.Left;
            }

            return value;
        }

        #endregion

        #region Input

        private void HandleInput() {
            // TODO: Check if we're in the map state.

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

            // TODO: Handle level selected
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

            levelTitleText.text = "We're moving!"; // TODO: Just clear the text.

            state = MapState.CursorMoving;
        }

        #endregion

        private bool DoesPathExist(int x, int y) {
            return pathDictionary.ContainsKey((x, y)) || levelDictionary.ContainsKey((x, y));
        }

        private bool DoesPathExist(int x, int y, Direction direction) {
            return DoesPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool DoesOpenPathExist(int x, int y) {
            if (pathDictionary.TryGetValue((x, y), out MapPathManager pathManager)) {
                return pathManager.IsOpen;
            }

            if (levelDictionary.TryGetValue((x, y), out MapLevelManager levelManager)) {
                return levelManager.IsOpen;
            }

            return false;
        }

        private bool DoesOpenPathExist(int x, int y, Direction direction) {
            return DoesOpenPathExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }
    }
}