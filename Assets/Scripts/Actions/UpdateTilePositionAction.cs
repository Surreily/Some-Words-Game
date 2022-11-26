public class UpdateTilePositionAction : IAction {
    private readonly ILevel board;
    private readonly int x;
    private readonly int y;
    private readonly Direction direction;

    public UpdateTilePositionAction(ILevel level, int x, int y, Direction direction) {
        this.board = level;
        this.x = x;
        this.y = y;
        this.direction = direction;
    }

    public void Do() {
        int newX = x + direction.GetXOffset();
        int newY = y + direction.GetYOffset();

        ITile tile = board.Tiles[x, y];

        tile.X = newX;
        tile.Y = newY;

        board.Tiles[newX, newY] = tile;
        board.Tiles[x, y] = null;
    }

    public void Undo() {
        int newX = x + direction.GetXOffset();
        int newY = y + direction.GetYOffset();

        ITile tile = board.Tiles[newX, newY];

        tile.X = x;
        tile.Y = y;

        board.Tiles[x, y] = tile;
        board.Tiles[newX, newY] = null;
    }
}
