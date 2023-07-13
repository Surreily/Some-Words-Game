using System;
using System.Collections.Generic;

namespace Surreily.SomeWords.Scripts.Json.Game {
    [Serializable]
    public class JsonGame {
        public string Name;
        public string Description;
        public int StartX;
        public int StartY;
        public List<JsonPath> Paths;
        public List<JsonLevel> Levels;
        public List<JsonDecoration> Decorations;
    }
}