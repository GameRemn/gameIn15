using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class SpriteEditor
{
    public static List<Sprite> CutTheSprite(Texture2D texture2D, Vector2Int parts)
    {
        List<Sprite> sprites = new List<Sprite>();
        Vector2 partSize = new Vector2(texture2D.width/parts.x, texture2D.height/parts.y);
        for (int y = 0; y < parts.y; y++)
        {
            for (int x = 0; x < parts.x; x++)
            {
                var newRectPosition = new Vector2(x * partSize.x, y * partSize.y);
                sprites.Add(Sprite.Create(texture2D, new Rect(newRectPosition, partSize), new Vector2(0.5f, 0.5f), 100f));
            }
        }
        return sprites;
    }
}
