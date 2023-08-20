using Surreily.SomeWords.Scripts.Model.Game;
using Surreily.SomeWords.Scripts.Utility;

namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class UpdateCursorPositionAction : IAction {
        private readonly CursorManager cursorManager;
        private readonly Direction direction;

        public UpdateCursorPositionAction(CursorManager cursorManager, Direction direction) {
            this.cursorManager = cursorManager;
            this.direction = direction;
        }

        public void Do() {
            cursorManager.X += direction.GetXOffset();
            cursorManager.Y += direction.GetYOffset();
        }

        public void Undo() {
            cursorManager.X -= direction.GetXOffset();
            cursorManager.Y -= direction.GetYOffset();
        }
    }
}
