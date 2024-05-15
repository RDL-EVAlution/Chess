using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLabelHandler
{
    public static void ChangeTurn()
    {
        if (Board.whiteTurn)
        {
            SceneData.instance.gameLabelText.text = "Whites Turn";
        }
        else
        {
            SceneData.instance.gameLabelText.text = "Blacks Turn";
        }
    }

    public static void Check()
    {
        SceneData.instance.gameLabelText.text += "\n Check";
    }

    public static void Checkmate()
    {
        if (Board.whiteTurn)
        {
            SceneData.instance.gameLabelText.text = "Blacks Win";
        }
        else
        {
            SceneData.instance.gameLabelText.text = "Whites Win";
        }
    }
}
