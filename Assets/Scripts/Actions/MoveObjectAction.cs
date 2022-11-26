public class MoveObjectAction : IAction {
    private readonly MovableBehaviour movableBehaviour;
    private readonly Direction direction;

    public MoveObjectAction(MovableBehaviour movableBehaviour, Direction direction) {
        this.movableBehaviour = movableBehaviour;
        this.direction = direction;
    }

    public void Do() {
        movableBehaviour.Move(direction);
    }

    public void Undo() {
        movableBehaviour.Move(direction.GetInverse());
    }
}