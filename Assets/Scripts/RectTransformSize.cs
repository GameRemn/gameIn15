using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformSize
{
    public static void SetRectTransformSize(RectTransform rectTransform, Vector2 size)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }

    public static void SetRectTransformSizeSquareOtherObj(RectTransform rectTransform, RectTransform objRectTransform, float scale = 1)
    {
        var objRect = objRectTransform.rect;
        var size = Mathf.Min(objRect.width, objRect.height) * scale;
        SetRectTransformSize(rectTransform, new Vector2(size, size));
    }
    public static void SetRectTransformSizeSquareOtherObj(RectTransform rectTransform, RectTransform objRectTransform, Vector2 scale)
    {
        var objRect = objRectTransform.rect;
        var size = Mathf.Min(objRect.width, objRect.height) * scale;
        SetRectTransformSize(rectTransform, new Vector2(size.x, size.y));
    }

    public static void SetRectTransformSizeOtherObj(RectTransform rectTransform, RectTransform objRectTransform, float scale = 1)
    {
        var objRect = objRectTransform.rect;
        var size = new Vector2(objRect.width, objRect.height) * scale;
        SetRectTransformSize(rectTransform, size);
    }
    public static void SetRectTransformSizeOtherObj(RectTransform rectTransform, RectTransform objRectTransform, Vector2 scale)
    {
        var objRect = objRectTransform.rect;
        var size = new Vector2(objRect.width * scale.x, objRect.height * scale.y);
        SetRectTransformSize(rectTransform, size);
    }
}
