public class PickUpOrDropTileAction : IAction {
    private IBoard board;

    public PickUpOrDropTileAction(IBoard board) {
        this.board = board;
    }

    public bool Do() {
        if (board.SelectedTile == null) {
            if (board.Tiles[board.CursorX, board.CursorY] != null) {
                PickUpTile();
                board.PlayInteractAudioClip();
                return true;
            }

            board.PlayInvalidAudioClip();
            return false;
        } else {
            if (board.Tiles[board.CursorX, board.CursorY] != null) {
                board.PlayInvalidAudioClip();
                return false;
            }

            DropTile();
            board.PlayInteractAudioClip();
            return true;
        }
        
    }

    public void Undo() {
        if (board.SelectedTile == null) {
            PickUpTile();
            
        } else {
            DropTile();
        }
    }

    private void PickUpTile() {
        board.SelectedTile = board.Tiles[board.CursorX, board.CursorY];
        board.Tiles[board.CursorX, board.CursorY] = null;
    }

    private void DropTile() {
        board.Tiles[board.CursorX, board.CursorY] = board.SelectedTile;
        board.SelectedTile = null;
    }
}
