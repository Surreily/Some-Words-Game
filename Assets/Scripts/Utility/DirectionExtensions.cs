using System;

public static class DirectionExtensions {
    public static int GetXOffset(this Direction direction) {
        switch (direction) {
            case Direction.Left:
                return -1;
            case Direction.Right:
                return 1;
            case Direction.Up:
            case Direction.Down:
                return 0;
            default:
                throw new InvalidOperationException("Unsupported direction.");
        }
    }

    public static int GetYOffset(this Direction direction) {
        switch (direction) {
            case Direction.Left:
            case Direction.Right:
                return 0;
            case Direction.Up:
                return -1;
            case Direction.Down:
                return 1;
            default:
                throw new InvalidOperationException("Unsupported direction.");
        }
    }

    public static Direction GetInverse(this Direction direction) {
        switch (direction) {
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            default:
                throw new InvalidOperationException("Unsupported direction.");
        }
    }
}
