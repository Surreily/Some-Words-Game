using System;

namespace Surreily.SomeWords.Scripts.Utility {
    [Flags]
    public enum PathTileType {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
    }
}
