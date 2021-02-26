using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lvl
{
    public Vector2Int numberOfColumnsAndRows;
    public Texture2D texture2D;
    public GameMode gameMode;
    public enum GameMode
    {
        Text,
        Image,
        TextAndImage
    }
}
