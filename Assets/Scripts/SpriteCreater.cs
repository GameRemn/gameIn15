using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCreater : MonoBehaviour
{
    public Texture2D tex;
    public List<Sprite> mySprite;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
    }

    void Start()
    {
        mySprite.Add(Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width/2, tex.height/2), new Vector2(0.5f, 0.5f), 100.0f));
        sr.sprite = mySprite[0];
    }
}
