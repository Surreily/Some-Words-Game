namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class DropTileAction : IAction {
        private readonly LevelManager levelManager;

        public DropTileAction(LevelManager levelManager) {
            this.levelManager = levelManager;
        }

        public void Do() {
            levelManager.State = LevelState.Normal;
        }

        public void Undo() {
            levelManager.State = LevelState.Selected;
        }
    }
}
