using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public static class Board
{
    private static Piece selectedPiece;

    private static int[] TilesData;

    public static TileView[] TilesView { get; private set; }

    public static string FEN = FENHandler.LOAD_FEN;

    public static bool whiteTurn = true;
    private static bool check = false;
    private static bool checkmate = false;

    public const int TOP_RIGHT = 63;
    public const int TOP_LEFT = 56;
    public const int BOTTOM_RIGHT = 7;
    public const int BOTTOM_LEFT = 0;

    public static void CreateBoard()
    {
        TilesData = FENHandler.LoadPositionFromFEN(FEN);

        TilesView = new TileView[TilesData.Length];

        whiteTurn = true;
        check = false;
        checkmate = false;
        GameLabelHandler.ChangeTurn();

        if (SceneData.instance.figuresParent.childCount > 0)
        {
            for (int i = 0;  i < SceneData.instance.figuresParent.childCount; i++)
            {
                GameObject.Destroy(SceneData.instance.figuresParent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < TilesData.Length; i++)
        {
            GameObject tileViewInstance = GameObject.Instantiate(PrefabsData.TileView, BoardAsIntHandler.TilePositionToVector3(i), Quaternion.identity, SceneData.instance.tilesViewsParent);
            TilesView[i] = tileViewInstance.GetComponent<TileView>();
            TilesView[i].position = i;
            TilesView[i].SetIdleColor();

            if (TilesData[i] != 0)
            {
                CreatePiece(i);
            }
        }
    }

    public static void Move(int targetPosition)
    {
        RemovePiece(targetPosition);

        TilesData[selectedPiece.position] = 0;
        selectedPiece.position = targetPosition;
        selectedPiece.transform.position = BoardAsIntHandler.TilePositionToVector3(targetPosition);
        TilesData[selectedPiece.position] = selectedPiece.type;

        ClearSelection();

        whiteTurn = !whiteTurn;
        GameLabelHandler.ChangeTurn();

        if (selectedPiece.type == (PieceAsIntHandler.Pawn | PieceAsIntHandler.White) && (TOP_LEFT <= targetPosition && targetPosition <= TOP_RIGHT))
        {
            RemovePiece(targetPosition);
            CreatePiece(PieceAsIntHandler.Queen | PieceAsIntHandler.White, targetPosition);
        }
        else if (selectedPiece.type == (PieceAsIntHandler.Pawn | PieceAsIntHandler.Black) && (BOTTOM_LEFT <= targetPosition && targetPosition <= BOTTOM_RIGHT))
        {
            RemovePiece(targetPosition);
            CreatePiece(PieceAsIntHandler.Queen | PieceAsIntHandler.Black, targetPosition);
        }

        check = Check(TilesData);

        if(check)
        {
            GameLabelHandler.Check();
            Checkmate();
        }
    }

    public static void ShowPath(Piece figure)
    {
        if (checkmate) return;

        ClearSelection();
        selectedPiece = figure;

        var bad_code_design = false;
        var paths = PieceMoveHandler.FindPaths(TilesData, selectedPiece.position, ref bad_code_design);

        SetTileViewAsSelected();

        foreach(var item in paths)
        {
            int[] tiles = new int[TilesData.Length];
            Array.Copy(TilesData, 0, tiles, 0, tiles.Length);
            tiles[selectedPiece.position] = 0;
            tiles[item] = selectedPiece.type;

            if (Check(tiles))
            {
                continue;
            }
            SetTileViewAsPath(item);
        }


    }

    private static void ClearSelection()
    {
        if (selectedPiece == null)
        {
            return;
        }

        foreach (var tileView in selectedPiece.tilesViews)
        {
            tileView.SetIdleColor();
        }
        selectedPiece.tilesViews.Clear();
    }

    private static void SetTileViewAsSelected()
    {
        TilesView[selectedPiece.position].SetSelectedColor();
        selectedPiece.tilesViews.Add(TilesView[selectedPiece.position]);
    }

    private static void SetTileViewAsPath(int position)
    {
        TilesView[position].SetPathColor();
        selectedPiece.tilesViews.Add(TilesView[position]);
    }

    private static void RemovePiece(int position)
    {
        //по хорошему выбитые фигуры должны сохраняться где-то сбоку от доски. Для этого нужен GridGroup
        TilesData[position] = 0;

        Physics2D.SyncTransforms();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(BoardAsIntHandler.TilePositionToVector3(position), 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Piece figure))
            {
                var parent = PieceAsIntHandler.Color(figure.type, PieceAsIntHandler.White) ? SceneData.instance.whiteBeatens : SceneData.instance.blackBeatens;
                GameObject figureInstance = GameObject.Instantiate(PrefabsData.PieceImage, parent);
                var imageComponent = figureInstance.GetComponent<Image>();
                imageComponent.sprite = PrefabsData.PieceToPrefab(figure.type);

                GameObject.Destroy(figure.gameObject);
            }
        }
    }

    private static void CreatePiece(int i)
    {
        GameObject figureInstance = GameObject.Instantiate(PrefabsData.Figure, BoardAsIntHandler.TilePositionToVector3(i), Quaternion.identity, SceneData.instance.figuresParent);
        var figure = figureInstance.GetComponent<Piece>();
        figure.spriteRenderer.sprite = PrefabsData.PieceToPrefab(TilesData[i]);
        figure.type = TilesData[i];
        figure.position = i;
    }

    private static void CreatePiece(int type, int position)
    {
        GameObject figureInstance = GameObject.Instantiate(PrefabsData.Figure, BoardAsIntHandler.TilePositionToVector3(position), Quaternion.identity, SceneData.instance.figuresParent);
        var figure = figureInstance.GetComponent<Piece>();
        figure.spriteRenderer.sprite = PrefabsData.PieceToPrefab(type);
        figure.type = type;
        figure.position = position;

        TilesData[position] = type;
    }

    private static bool Check(int[] tiles) //We are looking for a piece who can beat a king
    {
        var check = false;

        var color = !whiteTurn ? PieceAsIntHandler.White : PieceAsIntHandler.Black;

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != 0 && PieceAsIntHandler.Color(tiles[i], color))
            {
                PieceMoveHandler.FindPaths(tiles, i, ref check);

                if (check)
                {
                    break;
                }
            }
        }

        return check;
    }

    private static void Checkmate()
    {
        var color = whiteTurn ? PieceAsIntHandler.White : PieceAsIntHandler.Black;

        for (int i = 0; i < TilesData.Length; i++)
        {
            if (TilesData[i] != 0 && PieceAsIntHandler.Color(TilesData[i], color))
            {
                var bad_code_design = false;
                var paths = PieceMoveHandler.FindPaths(TilesData, i, ref bad_code_design);

                for (int j = 0; j < paths.Count; j++)
                {
                    int[] tiles = new int[TilesData.Length];
                    Array.Copy(TilesData, 0, tiles, 0, tiles.Length);
                    tiles[i] = 0;
                    tiles[paths[j]] = TilesData[i];

                    if (!Check(tiles))
                    {
                        return;
                    }
                }
            }
        }

        checkmate = true;
        GameLabelHandler.Checkmate();
    }
}