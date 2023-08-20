using System;

namespace Surreily.SomeWords.Scripts.Json.Game {
    [Serializable]
    public class JsonPath {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public int Colour;
        public bool IsHidden;
    }
}