public interface ILevel {
    int Width { get; }
    int Height { get; }
    ITile[,] Tiles { get; }

    int CursorX { get; set; }
    int CursorY { get; set; }
    CursorState CursorState { get; set; }

    void MoveCursor(Direction direction);

    bool IsPositionOnBoard(int x, int y);
}
