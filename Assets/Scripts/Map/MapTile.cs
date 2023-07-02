using Surreily.SomeWords.Scripts.Utility;

namespace Surreily.SomeWords.Scripts.Map {
    public class MapTile {
        public int Variation { get; set; }
        public PathTileType Position { get; set; }
        public bool IsOpen { get; set; }
    }
}
