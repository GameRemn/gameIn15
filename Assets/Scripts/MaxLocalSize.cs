using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class MaxLocalSize : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentRectTransform;
    public RectTransform ParentRectTransform
    {
        get => parentRectTransform;
        set
        {
            parentRectTransform = value;
            SetRectTransformSize();
        }
    }
    [SerializeField]
    private Vector2 scale = Vector2.one;
    public Vector2 Scale
    {
        get => scale;
        set
        {
            scale = value;
            SetRectTransformSize();
        }
    }
    [SerializeField]
    private bool asSquare = false;
    public bool AsSquare
    {
        get => asSquare;
        set
        {
            asSquare = value;
            SetRectTransformSize();
        }
    }

    private RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (!parentRectTransform)
            parentRectTransform = GetComponentInParent<RectTransform>();
        SetRectTransformSize();
    }

    void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            SetRectTransformSize();
        }
    }

    public void SetRectTransformSize()
    {
        if (_rectTransform && parentRectTransform)
        {
            if (!asSquare)
            {
                RectTransformSize.SetRectTransformSizeOtherObj(_rectTransform, parentRectTransform, scale);
            }
            else
            {
                RectTransformSize.SetRectTransformSizeSquareOtherObj(_rectTransform, parentRectTransform, scale);
            }
        }
    }
}
