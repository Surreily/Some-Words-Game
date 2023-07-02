using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapManager : MonoBehaviour {
        private Dictionary<(int, int), MapPathManager> pathDictionary;
        private Dictionary<(int, int), MapLevelManager> levelDictionary;
        private int cursorX;
        private int cursorY;

        public MaterialStore MaterialStore { get; set; }

        #region Load Map

        public void LoadMap(JsonMap map) {
            SetUpPathDictionary(map);
            SetUpLevelDictionary(map);

            CalculatePathTypes();
        }

        private void SetUpPathDictionary(JsonMap map) {
            pathDictionary = new Dictionary<(int, int), MapPathManager>();

            foreach (JsonMapPath path in map.Paths) {
                for (int x = 0; x < path.Width; x++) {
                    for (int y = 0; y < path.Height; y++) {
                        GameObject pathObject = new GameObject();
                        pathObject.transform.SetParent(gameObject.transform, false);
                        pathObject.transform.Translate(path.X + x, path.Y + y, Layers.MapPath, Space.Self);

                        MapPathManager pathManager = pathObject.AddComponent<MapPathManager>();
                        pathManager.MaterialStore = MaterialStore;
                        pathManager.Variation = 1; // TODO: Get from JSON.
                        pathManager.IsOpen = true; // TODO: Get from JSON or calculate.

                        pathDictionary.Add((path.X + x, path.Y + y), pathManager);
                    }
                }
            }
        }

        private void SetUpLevelDictionary(JsonMap map) {
            levelDictionary = new Dictionary<(int, int), MapLevelManager>();

            foreach (JsonMapLevel level in map.Levels) {
                GameObject levelObject = new GameObject();
                levelObject.transform.SetParent(gameObject.transform, false);
                levelObject.transform.Translate(level.X, level.Y, Layers.MapLevel, Space.Self);

                MapLevelManager levelManager = levelObject.AddComponent<MapLevelManager>();
                levelManager.MaterialStore = MaterialStore;
                levelManager.Variation = 1; // TODO: Get from JSON.
                levelManager.IsOpen = true; // TODO: Get from JSON or calculate.

                levelDictionary.Add((level.X, level.Y), levelManager);
            }
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

                pathManager.TileType = GetPathTileTile(up, right, down, left);
            }
        }

        private PathTileType GetPathTileTile(bool up, bool right, bool down, bool left) {
            if (up) {
                if (right) {
                    if (down) {
                        if (left) {
                            return PathTileType.All;
                        } else {
                            return PathTileType.VerticalAndRight;
                        }
                    } else {
                        if (left) {
                            return PathTileType.HorizontalAndUp;
                        } else {
                            return PathTileType.UpAndRight;
                        }
                    }
                } else {
                    if (down) {
                        if (left) {
                            return PathTileType.VerticalAndLeft;
                        } else {
                            return PathTileType.Vertical;
                        }
                    } else {
                        if (left) {
                            return PathTileType.UpAndRight;
                        }
                    }
                }
            } else {
                if (right) {
                    if (down) {
                        if (left) {
                            return PathTileType.HorizontalAndDown;
                        } else {
                            return PathTileType.DownAndRight;
                        }
                    } else {
                        if (left) {
                            return PathTileType.Horizontal;
                        }
                    }
                } else {
                    if (down && left) {
                        return PathTileType.DownAndLeft;
                    }
                }
            }

            return PathTileType.All; // TODO: Return an invalid result.
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

            // TODO: Actual movement.
            cursorX = newX;
            cursorY = newY;
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