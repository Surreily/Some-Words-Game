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
        public string State;
        public JsonColor Color;
        public int Width;
        public int Height;
        public int StartX;
        public int StartY;
        public List<JsonTile> Tiles;
        public JsonLevelGoal Goal;
    }
}