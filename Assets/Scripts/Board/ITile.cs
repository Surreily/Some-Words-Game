using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    MovableBehaviour MovableBehaviour { get; }
    TileBackgroundRenderer CharacterRenderer { get; }
    TileBackgroundRenderer BackgroundRenderer { get; }
    int X { get; set; }
    int Y { get; set; }
    char Character { get; }
}
