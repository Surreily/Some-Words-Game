namespace Surreily.SomeWords.Scripts.Utility {
    public static class PathTileTypeHelper {
        public static PathTileType GetPathTileType(bool up, bool right, bool down, bool left) {
            PathTileType value = PathTileType.None;

            if (up) {
                value |= PathTileType.Up;
            }

            if (right) {
                value |= PathTileType.Right;
            }

            if (down) {
                value |= PathTileType.Down;
            }

            if (left) {
                value |= PathTileType.Left;
            }

            return value;
        }
    }
}
