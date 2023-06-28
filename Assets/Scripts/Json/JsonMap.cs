using System;
using System.Collections.Generic;

[Serializable]
public class JsonMap {
    public int StartX;
    public int StartY;

    public List<JsonMapDecoration> Decorations;
    public List<JsonMapPath> Paths;
    public List<JsonMapLevel> Levels;
}
