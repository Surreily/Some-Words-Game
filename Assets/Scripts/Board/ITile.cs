using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    char? Character { get; }

    void Move(Direction direction);
}
