using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Utility;

namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class UpdateTilePositionAction : IAction {
        private readonly LevelManager levelManager;
        private readonly int x;
        private readonly int y;
        private readonly Direction direction;

        public UpdateTilePositionAction(LevelManager levelManager, int x, int y, Direction direction) {
            this.levelManager = levelManager;
            this.x = x;
            this.y = y;
            this.direction = direction;
        }

        public void Do() {
            int newX = x + direction.GetXOffset();
            int newY = y + direction.GetYOffset();

            TileManager tileManager = levelManager.TileManagers[x, y];

            tileManager.X = newX;
            tileManager.Y = newY;

            levelManager.TileManagers[newX, newY] = tileManager;
            levelManager.TileManagers[x, y] = null;
        }

        public void Undo() {
            int newX = x + direction.GetXOffset();
            int newY = y + direction.GetYOffset();

            TileManager tileManager = levelManager.TileManagers[newX, newY];

            tileManager.X = x;
            tileManager.Y = y;

            levelManager.TileManagers[x, y] = tileManager;
            levelManager.TileManagers[newX, newY] = null;
        }
    }
}