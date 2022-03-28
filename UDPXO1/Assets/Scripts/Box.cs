using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int index;
    public Mark mark;
    public bool isMarked;
    private SpriteRenderer spriterRnderer;

    private void Awake()
    {
        mark = Mark.None;
        spriterRnderer = GetComponent<SpriteRenderer>();
        index = transform.GetSiblingIndex();
        isMarked = false;
    }
    public void SetMarked (Sprite sprite, Mark mark, Color color)
    {
        isMarked = true;
        this.mark = mark;
        spriterRnderer.color = color;
        spriterRnderer.sprite = sprite;

        GetComponent<CircleCollider2D>().enabled = false;
    }
}
