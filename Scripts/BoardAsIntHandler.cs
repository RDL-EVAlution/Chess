using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardAsIntHandler
{
    private const float TILE_DISTANCE = 1.11f;

    // 56 57 58 59 60 61 62 63
    // 48 59 50 51 52 53 54 55
    // 40 41 42 43 44 45 46 47
    // 32 33 34 35 36 37 38 39
    // 24 25 26 27 28 29 30 31
    // 16 17 18 19 20 21 22 23
    //  8  9 10 11 12 13 14 15
    //  0  1  2  3  4  5  6  7

    public static int GetPositionRank(int position)
    {
        return position % 8;
    }

    public static int GetPositionFile(int position)
    {
        return position / 8;
    }

    public static Vector3 TilePositionToVector3(int position)
    {
        int rank = GetPositionRank(position);
        int file = GetPositionFile(position);

        return new Vector3(rank, file, 0) * TILE_DISTANCE;
    }
}
