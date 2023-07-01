using Surreily.SomeWords.Scripts.Utility;

class MoveCursorAction : IAction {
    private readonly ILevel level;
    private readonly Direction direction;

    public MoveCursorAction(ILevel level, Direction direction) {
        this.level = level;
        this.direction = direction;
    }

    public void Do() {
        level.MoveCursor(direction);
    }

    public void Undo() {
        level.MoveCursor(direction.GetInverse());
    }
}