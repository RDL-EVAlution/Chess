using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject point;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_camera.ScreenToWorldPoint(Input.mousePosition), 0.01f);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out TileView tileView))
                {
                    tileView.Click();
                    return;
                }
                else if (collider.TryGetComponent(out Piece figure))
                {
                    if (PieceAsIntHandler.Color(figure.type, PieceAsIntHandler.White) == Board.whiteTurn) //rightSelection => change selection
                    {
                        figure.Click();
                    }
                    else //wrongSelection => do nothing
                    {
                        continue;
                    }
                }
                else
                {
                    Debug.Log("Controller.TryGetComponent<> = false");
                }
            }
        }
    }
}
