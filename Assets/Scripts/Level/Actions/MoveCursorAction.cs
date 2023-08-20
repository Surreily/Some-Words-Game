using Surreily.SomeWords.Model.Game;
using Surreily.SomeWords.Scripts.Utility;

namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class MoveCursorAction : IAction {
        private readonly CursorManager cursorManager;
        private readonly Direction direction;

        public MoveCursorAction(CursorManager cursorManager, Direction direction) {
            this.cursorManager = cursorManager;
            this.direction = direction;
        }

        public void Do() {
            cursorManager.Move(direction);
        }

        public void Undo() {
            cursorManager.Move(direction.GetInverse());
        }
    }
}
