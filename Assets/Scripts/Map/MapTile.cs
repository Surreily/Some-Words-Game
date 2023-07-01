namespace Surreily.SomeWords.Scripts.Map {
    public class MapTile {
        public MapTile NextNorth { get; set; }
        public MapTile NextEast { get; set; }
        public MapTile NextSouth { get; set; }
        public MapTile NextWest { get; set; }
        public bool IsOpen { get; set; }
    }
}
