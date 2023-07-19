using System.Collections.Generic;

namespace Surreily.SomeWords.Scripts.Model.Game {
    public class GameModel {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public List<PathModel> Paths { get; set; }
        public List<LevelModel> Levels { get; set; }
        public List<DecorationModel> Decorations { get; set; }
    }
}
