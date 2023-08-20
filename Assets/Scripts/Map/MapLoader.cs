using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Utility;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapLoader {
        private readonly MapManager mapManager;
        private readonly GameModel game;

        private readonly Dictionary<(int, int), PathState> pathStates;
        private readonly Dictionary<(int, int), LevelState> levelStates;

        public MapLoader(MapManager mapManager, GameModel game) {
            this.mapManager = mapManager;
            this.game = game;

            pathStates = new Dictionary<(int, int), PathState>();
            levelStates = new Dictionary<(int, int), LevelState>();
        }

        public void Load() {
            SetInitialPathStates();
            SetInitialLevelStates();
            SetUpdatedTileStates();
            CreatePathManagers();
            CreateLevelManagers();
        }

        private void SetInitialPathStates() {
            foreach (PathModel path in game.Paths) {
                pathStates.Add((path.X, path.Y), PathState.Open); // TODO: Get isOpen from path.

            }
        }

        private void SetInitialLevelStates() {
            foreach (LevelModel level in game.Levels) {
                levelStates.Add((level.X, level.Y), LevelState.Open); // TODO: get isOpen from level.
            }
        }

        private void SetUpdatedTileStates() {
            // TODO: 'Flood fill' tile states based on whether levels have been cleared from the save file.
        }

        private void CreatePathManagers() {
            foreach (PathModel path in game.Paths) {
                PathTileType type = PathTileTypeHelper.GetPathTileType(
                    IsTileVisible(path.X, path.Y, Direction.Up),
                    IsTileVisible(path.X, path.Y, Direction.Right),
                    IsTileVisible(path.X, path.Y, Direction.Down),
                    IsTileVisible(path.X, path.Y, Direction.Left));

                mapManager.AddPathManager(path, path.X, path.Y, type, path.State);
            }
        }

        private void CreateLevelManagers() {
            foreach (LevelModel level in game.Levels) {
                mapManager.AddLevelManager(level, LevelState.Open);
            }
        }

        #region Utility

        private bool DoesTileExist(int x, int y) {
            return pathStates.ContainsKey((x, y)) || levelStates.ContainsKey((x, y));
        }

        private bool DoesTileExist(int x, int y, Direction direction) {
            return DoesTileExist(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        private bool IsTileVisible(int x, int y) {
            if (pathStates.TryGetValue((x, y), out PathState pathState)) {
                return pathState != PathState.Hidden;
            }

            if (levelStates.TryGetValue((x, y), out LevelState levelState)) {
                return levelState != LevelState.Hidden;
            }

            return false;
        }

        private bool IsTileVisible(int x, int y, Direction direction) {
            return IsTileVisible(x + direction.GetXOffset(), y + direction.GetYOffset());
        }

        #endregion

    }
}
