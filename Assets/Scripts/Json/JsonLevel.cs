using System;
using System.Collections.Generic;

namespace Surreily.SomeWords.Scripts.Json.Game {
    [Serializable]
    public class JsonLevel {
        public int X;
        public int Y;
        public string Id;
        public string Title;
        public string Description;
        public int Colour;
        public int Width;
        public int Height;
        public int StartX;
        public int StartY;
        public string TargetWord;
        public List<JsonTile> Tiles;
    }
}