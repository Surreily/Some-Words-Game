using System.Collections.Generic;

namespace Surreily.SomeWords.Scripts.Model.Game {
    public class LevelModel {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public LevelState State { get; set; }
        public int Colour { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public List<LevelTileModel> Tiles { get; set; }
        public LevelGoalModel Goal { get; set; }
    }
}
