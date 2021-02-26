using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    protected Cell cell;
    public Cell Cell
    {
        get => cell;
        set
        {
            if (cell != null)
            {
                cell.tile = null;
            }
            cell = value;
            value.tile = this;
        }
    }
}
