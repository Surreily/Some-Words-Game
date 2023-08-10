namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class PickUpTileAction : IAction {
        private readonly LevelManager levelManager;

        public PickUpTileAction(LevelManager levelManager) {
            this.levelManager = levelManager;
        }

        public void Do() {
            levelManager.State = LevelState.Selected;
        }

        public void Undo() {
            levelManager.State = LevelState.Normal;
        }
    }
}
