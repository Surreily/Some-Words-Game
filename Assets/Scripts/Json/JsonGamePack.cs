using System;
using System.Collections.Generic;

[Serializable]
public class JsonGamePack {
    public string Name;
    public string Description;

    public List<JsonLevel> Levels;
}