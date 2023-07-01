using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Renderers;
using Surreily.SomeWords.Scripts.Utility;
using UnityEngine;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapManager : MonoBehaviour {
        private Dictionary<(int, int), MapTile> _mapTileDictionary;

        public MaterialStore MaterialStore { get; set; }

        public void LoadMap(JsonMap map) {
            // Set up the map tile dictionary.
            _mapTileDictionary = new Dictionary<(int, int), MapTile>();

            foreach (JsonMapLevel level in map.Levels) {
                _mapTileDictionary.Add((level.X, level.Y), new MapTile {
                    IsOpen = true,
                });
            }

            foreach (JsonMapPath path in map.Paths) {
                for (int x = 0; x < path.Width; x++) {
                    for (int y = 0; y < path.Height; y++) {
                        _mapTileDictionary.Add((path.X + x, path.Y + y), new MapTile {
                            IsOpen = true,
                        });
                    }
                }
            }

            // Add game objects.
            foreach (JsonMapDecoration decoration in map.Decorations) {
                for (int x = 0; x < decoration.Width; x++) {
                    for (int y = 0; y < decoration.Height; y++) {
                        CreateDecoration(decoration.X + x, decoration.Y + y, decoration.Material);
                    }
                }
            }

            foreach (JsonMapPath path in map.Paths) {
                for (int x = 0; x < path.Width; x++) {
                    for (int y = 0; y < path.Height; y++) {
                        CreatePath(path.X + x, path.Y + y);
                    }
                }
            }

            foreach (JsonMapLevel level in map.Levels) {
                CreateLevel(level.X, level.Y, level.Id);
            }
        }

        private void CreateDecoration(int x, int y, string material) {
            GameObject decoration = new GameObject();
            decoration.transform.Translate(x, y, Layers.MapDecoration, Space.Self);

            TileRenderer tileRenderer = decoration.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.BorderMaterial; // TODO: Use decoration material.
        }

        private void CreatePath(int x, int y) {
            GameObject path = new GameObject();
            path.transform.Translate(x, y, Layers.MapPath, Space.Self);

            TileRenderer tileRenderer = path.AddComponent<TileRenderer>();

            bool north = _mapTileDictionary.ContainsKey((x, y + 1));
            bool east = _mapTileDictionary.ContainsKey((x + 1, y));
            bool south = _mapTileDictionary.ContainsKey((x, y - 1));
            bool west = _mapTileDictionary.ContainsKey((x - 1, y));

            PathTileSetPosition position = GetPathTileSetDirection(north, east, south, west);

            tileRenderer.Material = MaterialStore.GetPathMaterial(1, position);
        }

        private PathTileSetPosition GetPathTileSetDirection(bool north, bool east, bool south, bool west) {
            if (north) {
                if (east) {
                    if (south) {
                        if (west) {
                            return PathTileSetPosition.All;
                        } else {
                            return PathTileSetPosition.VerticalAndRight;
                        }
                    } else {
                        if (west) {
                            return PathTileSetPosition.HorizontalAndUp;
                        } else {
                            return PathTileSetPosition.UpAndRight;
                        }
                    }
                } else {
                    if (south) {
                        if (west) {
                            return PathTileSetPosition.VerticalAndLeft;
                        } else {
                            return PathTileSetPosition.Vertical;
                        }
                    } else {
                        if (west) {
                            return PathTileSetPosition.UpAndRight;
                        }
                    }
                }
            } else {
                if (east) {
                    if (south) {
                        if (west) {
                            return PathTileSetPosition.HorizontalAndDown;
                        } else {
                            return PathTileSetPosition.DownAndRight;
                        }
                    } else {
                        if (west) {
                            return PathTileSetPosition.Horizontal;
                        }
                    }
                } else {
                    if (south && west) {
                        return PathTileSetPosition.DownAndLeft;
                    }
                }
            }

            return PathTileSetPosition.All; // TODO: Return an invalid result.
        }

        private void CreateLevel(int x, int y, string id) {
            GameObject level = new GameObject();
            level.transform.Translate(x, y, Layers.MapLevel, Space.Self);

            TileRenderer tileRenderer = level.AddComponent<TileRenderer>();
            tileRenderer.Material = MaterialStore.GetOpenLevelMaterial(0); // TODO: Pass in the variation.
        }
    }
}