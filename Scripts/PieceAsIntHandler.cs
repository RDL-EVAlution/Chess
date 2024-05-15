using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PieceAsIntHandler
{
    public const int None = 0;
    public const int Bishop = 1;
    public const int King = 2;
    public const int Knight = 3;
    public const int Pawn = 4;
    public const int Queen = 5;
    public const int Rook = 6;

    public const int White = 8;
    public const int Black = 16;

    public static int GetType(int type)
    {
        return (type & 7);
    }

    public static bool Color(int type, int color)
    {
        return (type & 24) == color;
    }

    public static bool IsColor(int type, int path)
    {
        return (type & 24) == (path & 24);
    }
}
