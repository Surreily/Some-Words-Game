public class UpdateTileStateAction : IAction {
    private readonly ITile tile;
    private readonly TileState oldTileState;
    private readonly TileState newTileState;

    public UpdateTileStateAction(ITile tile, TileState tileState) {
        this.tile = tile;
        oldTileState = tile.TileState;
        newTileState = tileState;
    }

    public void Do() {
        tile.TileState = newTileState;
    }

    public void Undo() {
        tile.TileState = oldTileState;
    }
}
