using System;

public class MoveCursorAction : IAction {
    private IBoard board;
    private Direction direction;

    public MoveCursorAction(IBoard board, Direction direction) {
        this.board = board;
        this.direction = direction;
    }

    public bool Do() {
        if (board.SelectedTile == null) {
            if (
                (direction == Direction.Left && board.CursorX == 0) ||
                (direction == Direction.Right && board.CursorX - 1 == board.Width) ||
                (direction == Direction.Up && board.CursorY - 1 == board.Height) ||
                (direction == Direction.Down && board.CursorY == 0)) {

                board.PlayInvalidAudioClip();
                return false;
            }

            board.MoveCursor(direction);
            board.PlayCursorAudioClip();
            return true;
        } else {
            // TODO: Move cursor.
            board.PlayInvalidAudioClip();
            return false;
        }
    }

    public void Undo() {
        if (board.SelectedTile == null) {
            board.MoveCursor(GetInverseOfDirection(direction));
        } else {
            // TODO: Move cursor.
        }
    }

    private Direction GetInverseOfDirection(Direction direction) {
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
                throw new ArgumentException("Invalid direction.", nameof(direction));
        }
    }
}
