public class Level : ILevel {
    

    public Level(int width, int height, int cursorX, int cursorY) {
        Width = width;
        Height = height;
        CursorX = cursorX;
        CursorY = cursorY;

        Tiles = new ITile[width, height];
        CursorState = CursorState.Normal;
    }

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int CursorX { get; set; }
    public int CursorY { get; set; }
    public ITile[,] Tiles { get; private set; }
    public CursorState CursorState { get; set; }

    public MovableBehaviour CursorMovableBehaviour { get; set; }

    public void MoveCursor(Direction direction) {
        CursorMovableBehaviour.Move(direction);
    }

    public bool IsPositionOnBoard(int x, int y) {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}
