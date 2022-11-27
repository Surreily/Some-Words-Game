using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    int X { get; set; }

    int Y { get; set; }

    char Character { get; }

    TileState TileState { get; set; }

    void AnimateMove(Direction direction);
}
