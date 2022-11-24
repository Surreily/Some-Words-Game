using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile {
    int X { get; }
    int Y { get; }
    char Character { get; }
}
