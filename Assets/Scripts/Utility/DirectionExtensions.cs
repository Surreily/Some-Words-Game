using System;
using Surreily.SomeWords.Scripts.Model.Game;

namespace Surreily.SomeWords.Scripts.Utility {
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
                    return 1;
                case Direction.Down:
                    return -1;
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

        public static Direction GetNextClockwise(this Direction direction) {
            switch (direction) {
                case Direction.Left:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Down;
                case Direction.Up:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Left;
                default:
                    throw new InvalidOperationException("Unsupported direction.");
            }
        }

        public static Direction GetNextAnticlockwise(this Direction direction) {
            switch (direction) {
                case Direction.Left:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Right;
                default:
                    throw new InvalidOperationException("Unsupported direction.");
            }
        }
    }
}