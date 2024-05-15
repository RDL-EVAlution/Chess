using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionView : MonoBehaviour
{
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        ChangeScale(1.2f);
    }

    private void OnMouseExit()
    {
        ChangeScale(1.0f);
    }

    private void OnMouseDown()
    {
        ChangeScale(0.8f);
    }

    private void OnMouseUp()
    {
        ChangeScale(1.2f);
    }

    private void ChangeScale(float scale)
    {
        transform.localScale = originalScale * scale;
    }
}
