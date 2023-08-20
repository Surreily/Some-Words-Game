using Surreily.SomeWords.Model.Game;
using Surreily.SomeWords.Scripts.Utility;

namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class MoveTileAction : IAction {
        private readonly TileManager tileManager;
        private readonly Direction direction;

        public MoveTileAction(TileManager tileManager, Direction direction) {
            this.tileManager = tileManager;
            this.direction = direction;
        }

        public void Do() {
            tileManager.Move(direction);
        }

        public void Undo() {
            tileManager.Move(direction.GetInverse());
        }
    }
}
