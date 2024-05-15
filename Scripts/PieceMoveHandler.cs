using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public static class PieceMoveHandler
{
    private static int[] BishopDirections = { 9, -7, -9, 7 };
    private static int[] RookDirections = { 8, 1, -8, -1 };
    private static int[] KnightMovesRank = { 15, 17, -15, -17 };
    private static int[] KnightMovesFile = { 10, -6, -10, 6 };

    public static List<int> FindPaths(int[] tiles, int position, ref bool check)
    {
        var moves = new List<int>();

        switch (PieceAsIntHandler.GetType(tiles[position]))
        {
            case PieceAsIntHandler.Bishop:

                moves.AddRange(BishopPaths(tiles, position, ref check));
                break;

            case PieceAsIntHandler.King:

                moves.AddRange(KingPaths(tiles, position, ref check));
                break;

            case PieceAsIntHandler.Knight:

                moves.AddRange(KnightPaths(tiles, position, ref check));
                break;

            case PieceAsIntHandler.Pawn:

                moves.AddRange(PawnPaths(tiles, position, ref check));
                break;

            case PieceAsIntHandler.Queen:

                moves.AddRange(BishopPaths(tiles, position, ref check));
                moves.AddRange(RookPaths(tiles, position, ref check));
                break;

            case PieceAsIntHandler.Rook:

                moves = RookPaths(tiles, position, ref check);
                break;
        }

        return moves;
    }

    private static List<int> BishopPaths(int[] tiles, int position, ref bool check)
    {
        var moves = new List<int>();

        foreach (var direction in BishopDirections)
        {
            var rank = BoardAsIntHandler.GetPositionRank(position);
            var file = BoardAsIntHandler.GetPositionFile(position);

            var path = position + direction;

            while (0 <= path && path <= 63)
            {
                var pathRank = BoardAsIntHandler.GetPositionRank(path);
                var pathFile = BoardAsIntHandler.GetPositionFile(path);

                if (math.abs(pathRank - rank) != 1 || math.abs(pathFile - file) != 1)
                {
                    break;
                }

                rank = pathRank;
                file = pathFile;

                if (PieceCheck(path, tiles))
                {
                    if (EnemyCheck(path, tiles[position], tiles, ref check))
                    {
                        moves.Add(path);
                    }
                    break;
                }
                moves.Add(path);

                path += direction;
            }
        }

        return moves;
    }

    private static List<int> KingPaths(int[] tiles, int position, ref bool check)
    {
        var moves = new List<int>();

        foreach (var direction in BishopDirections)
        {
            MoveKing(ref moves, tiles, position, direction, true, ref check);
        }

        foreach (var direction in RookDirections)
        {
            MoveKing(ref moves, tiles, position, direction, false, ref check);
        }

        return moves;
    }

    private static List<int> RookPaths(int[] tiles, int position, ref bool check)
    {
        var moves = new List<int>();

        foreach(var direction in RookDirections)
        {
            var distance = math.abs(direction) == 8 ? BoardAsIntHandler.GetPositionFile(position) : BoardAsIntHandler.GetPositionRank(position);

            distance = direction > 0 ? 7 - distance : distance;

            for (int i = 1; i <= distance; i++) 
            {
                var path = position + i * direction;

                if(PieceCheck(path, tiles))
                {
                    if (EnemyCheck(path, tiles[position], tiles, ref check))
                    {
                        moves.Add(path);
                    }
                    break;
                }
                moves.Add(path);
            }
        }

        return moves;
    }

    private static List<int> KnightPaths(int[] tiles, int position, ref bool check)
    {
        var moves = new List<int>();

        foreach (var direction in KnightMovesRank)
        {
            MoveKnight(ref moves, tiles, position, direction, 1, 2, ref check);
        }

        foreach (var direction in KnightMovesFile)
        {
            MoveKnight(ref moves, tiles, position, direction, 2, 1, ref check);
        }

        return moves;
    }

    private static List<int> PawnPaths(int[] tiles, int position, ref bool check)
    {
        var moves = new List<int>();

        var rank = BoardAsIntHandler.GetPositionRank(position);

        if (PieceAsIntHandler.Color(tiles[position], PieceAsIntHandler.White))
        {
            if (!PieceCheck(position + 8, tiles))
            { 
                moves.Add(position + 8);

                if (8 <= position && position <= 15)
                {
                    if (!PieceCheck(position + 16, tiles)) moves.Add(position + 16);
                }
            }

            MoveDiagonalPawn(ref moves, tiles, position, 9, rank, ref check);
            MoveDiagonalPawn(ref moves, tiles, position, 7, rank, ref check);
        }
        else
        {
            if (!PieceCheck(position - 8, tiles))
            {
                moves.Add(position - 8);

                if (48 <= position && position <= 55)
                {
                    if (!PieceCheck(position - 16, tiles)) moves.Add(position - 16);
                }
            }

            MoveDiagonalPawn(ref moves, tiles, position, -9, rank, ref check);
            MoveDiagonalPawn(ref moves, tiles, position, -7, rank, ref check);
        }

        return moves;
    }

    private static bool EnemyCheck(int path, int type, int[] tiles, ref bool check)
    {
        if (0 <= path && path <= 63)
        {
            if (tiles[path] != 0)
            {
                if (!PieceAsIntHandler.IsColor(tiles[path], type))
                {
                    if (PieceAsIntHandler.GetType(tiles[path]) == PieceAsIntHandler.King)
                    {
                        check = true;
                    }
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }
        else { return false; }
    }

    private static bool PieceCheck(int path, int[] tiles)
    {
        if (0 <= path && path <= 63)
        {
            if (tiles[path] != 0)
            {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }

    private static void MoveKing(ref List<int> moves, int[] tiles, int position, int direction, bool vertical, ref bool check)
    {
        var rank = BoardAsIntHandler.GetPositionRank(position);
        var file = BoardAsIntHandler.GetPositionFile(position);

        var path = position + direction;

        var pathRank = BoardAsIntHandler.GetPositionRank(path);
        var pathFile = BoardAsIntHandler.GetPositionFile(path);

        if (path > 63 || path < 0)
        {
            return;
        }

        if (vertical)
        {
            if (math.abs(pathRank - rank) != 1 || math.abs(pathFile - file) != 1)
            {
                return;
            }
        }
        else
        {
            if (math.abs(pathRank - rank) > 0 && math.abs(pathFile - file) > 0)
            {
                return;
            }
        }

        if (PieceCheck(path, tiles))
        {
            if (EnemyCheck(path, tiles[position], tiles, ref check))
            {
                moves.Add(path);
            }
            return;
        }
        moves.Add(path);
    }

    private static void MoveKnight(ref List<int> moves, int[] tiles, int position, int direction, int rankDif, int fileDif, ref bool check)
    {
        var rank = BoardAsIntHandler.GetPositionRank(position);
        var file = BoardAsIntHandler.GetPositionFile(position);

        var path = position + direction;

        var pathRank = BoardAsIntHandler.GetPositionRank(path);
        var pathFile = BoardAsIntHandler.GetPositionFile(path);

        if (path > 63 || path < 0)
        {
            return;
        }

        if (math.abs(pathRank - rank) != rankDif && math.abs(pathFile - file) != fileDif)
        {
            return;
        }

        if (PieceCheck(path, tiles))
        {
            if (EnemyCheck(path, tiles[position], tiles, ref check))
            {
                moves.Add(path);
            }
            return;
        }
        moves.Add(path);
    }

    private static void MoveDiagonalPawn(ref List<int> moves, int[] tiles, int position, int direction, int rank, ref bool check)
    {
        var path = position + direction;
        var pathRank = BoardAsIntHandler.GetPositionRank(path);
        if (EnemyCheck(path, tiles[position], tiles, ref check) && (math.abs(pathRank - rank) == 1)) moves.Add(path);
    }
}
