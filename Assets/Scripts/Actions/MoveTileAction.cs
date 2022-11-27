public class MoveTileAction : IAction {
    private readonly ITile tile;
    private readonly Direction direction;

    public MoveTileAction(ITile tile, Direction direction) {
        this.tile = tile;
        this.direction = direction;
    }

    public void Do() {
        tile.AnimateMove(direction);
    }

    public void Undo() {
        tile.AnimateMove(direction.GetInverse());
    }
}