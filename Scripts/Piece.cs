using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// View and model in single class, because its anyway small
/// </summary>
public class Piece : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int type;
    public int position;

    public List<TileView> tilesViews;

    public void Click()
    {
        Board.ShowPath(this);
    }
}