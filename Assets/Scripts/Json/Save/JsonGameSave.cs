using System;
using System.Collections.Generic;

namespace Surreily.SomeWords.Scripts.Json.Save {
    [Serializable]
    public class JsonGameSave {
        public List<JsonLevelSave> LevelSaves;
    }
}
