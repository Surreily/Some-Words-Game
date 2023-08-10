using System.Collections.Generic;
using Surreily.SomeWords.Scripts.Materials;
using Surreily.SomeWords.Scripts.Model.Game;

namespace Surreily.SomeWords.Scripts {
    public interface IGameManager {
        public MaterialStore MaterialStore { get; }
        public HashSet<string> Dictionary { get; }
        public CameraMovement CameraMovement { get; }

        public GameState State { get; }

        public void OpenMap(GameModel game);
        public void CloseMap();
        public void OpenLevel(LevelModel level);
        public void CloseLevel();
    }
}
