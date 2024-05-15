using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public static SceneData instance = null;

    public GameObject board;
    public GameObject menuButtons;
    public GameObject gameButtons;
    public TextMeshProUGUI gameLabelText;
    public TextMeshProUGUI startButton;

    public Transform figuresParent;
    public Transform tilesViewsParent;

    public Transform whiteBeatens;
    public Transform blackBeatens;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
