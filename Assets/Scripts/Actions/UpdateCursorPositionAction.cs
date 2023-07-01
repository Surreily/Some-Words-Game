using Surreily.SomeWords.Scripts.Utility;

public class UpdateCursorPositionAction : IAction {
    private readonly ILevel level;
    private readonly Direction direction;

    public UpdateCursorPositionAction(ILevel level, Direction direction) {
        this.level = level;
        this.direction = direction;
    }

    public void Do() {
        level.CursorX += direction.GetXOffset();
        level.CursorY += direction.GetYOffset();
    }

    public void Undo() {
        level.CursorX -= direction.GetXOffset();
        level.CursorY -= direction.GetYOffset();
    }
}
