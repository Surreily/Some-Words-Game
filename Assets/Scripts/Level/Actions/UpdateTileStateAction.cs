namespace Surreily.SomeWords.Scripts.Level.Actions {
    public class UpdateTileStateAction : IAction {
        private readonly TileManager tileManager;
        private readonly TileState oldTileState;
        private readonly TileState newTileState;

        public UpdateTileStateAction(TileManager tileManager, TileState tileState) {
            this.tileManager = tileManager;

            oldTileState = tileManager.State;
            newTileState = tileState;
        }

        public void Do() {
            tileManager.State = newTileState;
        }

        public void Undo() {
            tileManager.State = oldTileState;
        }
    }
}
