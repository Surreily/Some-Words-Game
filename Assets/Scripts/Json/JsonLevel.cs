using System;
using System.Collections.Generic;

[Serializable]
public class JsonLevel {
    public string Title;
    public string Description;
    public int Width;
    public int Height;
    public int CursorX;
    public int CursorY;
    public string TargetWord;

    public List<JsonTile> Tiles;
}
