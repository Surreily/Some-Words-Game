public class PickUpOrDropTileAction : IAction {
    private IBoard board;

    public PickUpOrDropTileAction(IBoard board) {
        this.board = board;
    }

    public bool Do() {
        if (board.CursorState == CursorState.Normal) {
            if (board.GetTileUnderCursor() != null) {
                board.CursorState = CursorState.Selected;
                board.PlayInteractAudioClip();
                return true;
            } else {
                board.PlayInvalidAudioClip();
                return false;
            }
        } else {
            board.CursorState = CursorState.Normal;
            board.PlayInteractAudioClip();
            return true;
        }
    }

    public void Undo() {
        board.CursorState = board.CursorState == CursorState.Normal
            ? CursorState.Selected
            : CursorState.Normal;
    }
}
