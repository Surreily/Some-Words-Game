public class DropTileAction : IAction {
    private readonly ILevel level;

    public DropTileAction(ILevel level) {
        this.level = level;
    }

    public void Do() {
        level.CursorState = CursorState.Normal;
    }

    public void Undo() {
        level.CursorState = CursorState.Selected;
    }
}
