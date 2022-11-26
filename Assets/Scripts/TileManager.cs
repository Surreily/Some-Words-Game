public class Tile : ITile {
    public MovableBehaviour MovableBehaviour { get; set; }
    public TileBackgroundRenderer CharacterRenderer { get; set; }
    public TileBackgroundRenderer BackgroundRenderer { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public char Character { get; set; }
}
