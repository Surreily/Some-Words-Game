public class PickUpTileAction : IAction {
    private readonly ILevel level;

    public PickUpTileAction(ILevel level) {
        this.level = level;
    }

    public void Do() {
        level.CursorState = CursorState.Selected;
    }

    public void Undo() {
        level.CursorState = CursorState.Normal;
    }
}