using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FENHandler
{
    public readonly static string LOAD_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    public readonly static Dictionary<char, int> pieceTypeFromSymbol = new Dictionary<char, int>()
    {
        ['b'] = PieceAsIntHandler.Bishop,
        ['k'] = PieceAsIntHandler.King,
        ['n'] = PieceAsIntHandler.Knight,
        ['p'] = PieceAsIntHandler.Pawn,
        ['q'] = PieceAsIntHandler.Queen,
        ['r'] = PieceAsIntHandler.Rook,
    };

    private readonly static Dictionary<int, char> pieceTypeFromInt = new Dictionary<int, char>()
    {
        [PieceAsIntHandler.Bishop] = 'b',
        [PieceAsIntHandler.King] = 'k',
        [PieceAsIntHandler.Knight] = 'n',
        [PieceAsIntHandler.Pawn] = 'p',
        [PieceAsIntHandler.Queen] = 'q',
        [PieceAsIntHandler.Rook] = 'r',
    };

    public static int[] LoadPositionFromFEN(string FEN)
    {
        var tiles = new int[64];

        string fenBoard = FEN.Split(' ')[0];
        int file = 0, rank = 7;

        foreach (char symbol in fenBoard)
        {
            if (symbol == '/')
            {
                file = 0;
                rank--;

            }
            else if (char.IsDigit(symbol))
            {
                file += (int)char.GetNumericValue(symbol);
            }
            else
            {
                int pieceColour = (char.IsUpper(symbol)) ? PieceAsIntHandler.White : PieceAsIntHandler.Black;
                int pieceType = pieceTypeFromSymbol[char.ToLower(symbol)];
                tiles[rank * 8 + file] = pieceType | pieceColour;
                file++;
            }
        }

        return tiles;
    }

    public static string LoadFENFromPositions(int[] tiles) //Есть проблема с тем что оно считает строки не -> а <-, из-за чего получается зеркальное отражение по оси Y
    {
        string result = string.Empty;

        int file = 0, rank = 7, space = 0;



        for (int i = tiles.Length - 1; i >= 0; i--)
        {
            if (rank == -1)
            {
                break;
            }

            if (file == 8)
            {
                if (space > 0)
                {
                    result += space.ToString();
                    space = 0;
                }

                result += '/';
                rank--;

                file = 0;
            }

            if(tiles[i] == 0)
            {
                space++;
                file++;
            }
            else
            {
                if(space > 0)
                {
                    result += space.ToString();
                    space = 0;
                }

                int type = PieceAsIntHandler.GetType(tiles[i]);

                bool isUpper = PieceAsIntHandler.Color(tiles[i], PieceAsIntHandler.White) ? true : false;
                char key = pieceTypeFromInt[type];
                if (isUpper) key = char.ToUpper(key);
                result += key;

                file++;
            }
        }

        return result;
    }
}