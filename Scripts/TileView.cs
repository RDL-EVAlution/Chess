using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileView : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color idleColor;
    [SerializeField] private Color pathToMoveColor;
    [SerializeField] private Color selectedColor;

    public int position;
    private bool clickable = false;

    public void Click()
    {
        if (!clickable)
        {
            return;
        }

        Board.Move(position);
    }

    public void SetIdleColor()
    {
        spriteRenderer.color = idleColor;
        clickable = false;
        boxCollider.enabled = clickable;
    }

    public void SetPathColor()
    {
        spriteRenderer.color = pathToMoveColor;
        clickable = true;
        boxCollider.enabled = clickable;
    }

    public void SetSelectedColor()
    {
        spriteRenderer.color = selectedColor;
        clickable = false;
        boxCollider.enabled = clickable;
    }
}