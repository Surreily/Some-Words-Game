using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoard {
    public int Width { get; set; }
    public int Height { get; set; }
    public int CursorX { get; set; }
    public int CursorY { get; set; }
    public ITile[,] Tiles { get; set; }
    public ITile SelectedTile { get; set; }

    public void MoveCursor(Direction direction);
    public void PlayCursorAudioClip();
    public void PlayInteractAudioClip();
    public void PlayInvalidAudioClip();
}
