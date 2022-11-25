using System;
using System.Collections.Generic;

public class MoveCursorAction : IAction {
    private IBoard board;
    private Direction direction;

    private List<ITile> tiles;

    public MoveCursorAction(IBoard board, Direction direction) {
        this.board = board;
        this.direction = direction;

        tiles = new List<ITile>();
    }

    public bool Do() {
        if (board.CursorState == CursorState.Normal) {
            if (
                (direction == Direction.Left && board.CursorX == 0) ||
                (direction == Direction.Right && board.CursorX == board.Width - 1) ||
                (direction == Direction.Up && board.CursorY == 0) ||
                (direction == Direction.Down && board.CursorY == board.Height - 1)) {

                board.PlayInvalidAudioClip();
                return false;
            }

            board.MoveCursor(direction);
            board.PlayCursorAudioClip();
            return true;
        } else {
            if (TryMoveTiles()) {
                board.PlayCursorAudioClip();
                return true;
            } else {
                board.PlayInvalidAudioClip();
                return false;
            }
        }
    }

    public void Undo() {
        Direction inverseDirection = GetInverseOfDirection();

        board.MoveCursor(inverseDirection);

        if (board.CursorState == CursorState.Selected) {
            foreach (ITile tile in tiles) {
                board.MoveTile(tile.X, tile.Y, inverseDirection);
            }
        }
    }

    private bool TryMoveTiles() {
        int x = board.CursorX;
        int y = board.CursorY;
        int xStep = GetXStepForDirection();
        int yStep = GetYStepForDirection();

        while (true) {
            // If we reached the edge of the board, return false.
            if (x < 0 || x >= board.Width || y < 0 || y >= board.Height) {
                return false;
            }

            // Get the tile.
            ITile tile = board.GetTile(x, y);

            // TODO: If tile is immovable, return false.

            // If no tile at position, proceed to movement.
            if (tile == null) {
                break;
            }

            // Add tile to list.
            tiles.Add(tile);

            // Update X and Y.
            x += xStep;
            y += yStep;
        }

        // Move tiles. Must be done in reverse to avoid interfering with each other.
        for (int i = tiles.Count - 1; i >= 0; i--) {
            ITile tile = tiles[i];
            board.MoveTile(tile.X, tile.Y, direction);
        }

        board.MoveCursor(direction);

        return true;
    }

    private int GetXStepForDirection() {
        switch (direction) {
            case Direction.Left:
                return -1;
            case Direction.Right:
                return 1;
            case Direction.Up:
            case Direction.Down:
                return 0;
            default:
                throw new ArgumentException("Unsupported direction.", nameof(direction));
        }
    }

    private int GetYStepForDirection() {
        switch (direction) {
            case Direction.Left:
            case Direction.Right:
                return 0;
            case Direction.Up:
                return -1;
            case Direction.Down:
                return 1;
            default:
                throw new ArgumentException("Unsupported direction.", nameof(direction));
        }
    }

    private Direction GetInverseOfDirection() {
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
                throw new ArgumentException("Unsupported direction.", nameof(direction));
        }
    }
}
