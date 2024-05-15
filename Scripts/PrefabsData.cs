using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabsData
{
    public static GameObject TileView = Resources.Load<GameObject>("Prefabs/TileView");

    public static GameObject Figure = Resources.Load<GameObject>("Prefabs/Figure");
    public static GameObject PieceImage = Resources.Load<GameObject>("Prefabs/PieceImage");

    private static Sprite WhiteBishop = Resources.Load<Sprite>("Sprites/Figures/White/WhiteBishop");
    private static Sprite WhiteKing = Resources.Load<Sprite>("Sprites/Figures/White/WhiteKing");
    private static Sprite WhiteKnight = Resources.Load<Sprite>("Sprites/Figures/White/WhiteKnight");
    private static Sprite WhitePawn = Resources.Load<Sprite>("Sprites/Figures/White/WhitePawn");
    private static Sprite WhiteQueen = Resources.Load<Sprite>("Sprites/Figures/White/WhiteQueen");
    private static Sprite WhiteRook = Resources.Load<Sprite>("Sprites/Figures/White/WhiteRook");

    private static Sprite BlackBishop = Resources.Load<Sprite>("Sprites/Figures/Black/BlackBishop");
    private static Sprite BlackKing = Resources.Load<Sprite>("Sprites/Figures/Black/BlackKing");
    private static Sprite BlackKnight = Resources.Load<Sprite>("Sprites/Figures/Black/BlackKnight");
    private static Sprite BlackPawn = Resources.Load<Sprite>("Sprites/Figures/Black/BlackPawn");
    private static Sprite BlackQueen = Resources.Load<Sprite>("Sprites/Figures/Black/BlackQueen");
    private static Sprite BlackRook = Resources.Load<Sprite>("Sprites/Figures/Black/BlackRook");

    public static Sprite PieceToPrefab(int type)
    {
        switch (type)
        {
            case PieceAsIntHandler.None:
                return null;

            case PieceAsIntHandler.Bishop | PieceAsIntHandler.White:
                return WhiteBishop;
            case PieceAsIntHandler.King | PieceAsIntHandler.White:
                return WhiteKing;
            case PieceAsIntHandler.Knight | PieceAsIntHandler.White:
                return WhiteKnight;
            case PieceAsIntHandler.Pawn | PieceAsIntHandler.White:
                return WhitePawn;
            case PieceAsIntHandler.Queen | PieceAsIntHandler.White:
                return WhiteQueen;
            case PieceAsIntHandler.Rook | PieceAsIntHandler.White:
                return WhiteRook;

            case PieceAsIntHandler.Bishop | PieceAsIntHandler.Black:
                return BlackBishop;
            case PieceAsIntHandler.King | PieceAsIntHandler.Black:
                return BlackKing;
            case PieceAsIntHandler.Knight | PieceAsIntHandler.Black:
                return BlackKnight;
            case PieceAsIntHandler.Pawn | PieceAsIntHandler.Black:
                return BlackPawn;
            case PieceAsIntHandler.Queen | PieceAsIntHandler.Black:
                return BlackQueen;
            case PieceAsIntHandler.Rook | PieceAsIntHandler.Black:
                return BlackRook;

            default:
                return null;
        }
    }
}
