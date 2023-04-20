using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PelletState
{
    public List<Vector3Int> pelletPositions;

    public PelletState(List<Vector3Int> pelletPositions)
    {
        this.pelletPositions = pelletPositions;
    }
}

