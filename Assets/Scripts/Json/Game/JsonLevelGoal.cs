using System;

namespace Surreily.SomeWords.Scripts.Json.Game {
    [Serializable]
    public class JsonLevelGoal {
        public string Id;
        public string Type;
        public string Word;
        public string[] Directions;
    }
}
