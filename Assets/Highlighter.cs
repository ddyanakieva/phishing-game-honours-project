using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highlighter : MonoBehaviour
{   
    private Texture2D cursorTexture;
    private Vector2 cursorHotspot;

    private SpriteRenderer sprite;
    private Color originalColors, currColor, clickedColor;
    private float timeMultiplier = 2f;
    private bool clicked = false, highlighting = false;

    public void Initialize()
    {
        cursorTexture = Resources.Load<Texture2D>("cursor-fish3");
        cursorHotspot = new Vector2(cursorTexture.width / 4, cursorTexture.height / 3);
        sprite = GetComponent<SpriteRenderer>();
        originalColors = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.7f);
        clickedColor = new Color(0.4316927f, 0.552348f, 0.8396226f, 0.8f);
    }

    public void OnMouseEnter()
    {
        originalColors.a = 0.7f;
        sprite.color = originalColors;
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);

        ShowURLTextAbove(true);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        ShowURLTextAbove(false);

        if (currColor.a == 0.8f)
        {
            sprite.color = currColor;
            return;
        } 

        originalColors.a = 0;
        sprite.color = originalColors;
    }

    // OnMouseClick
    public void OnMouseDown()
    {
        clicked = !clicked;
        ShowURLTextAbove(false);

        currColor = clicked ? clickedColor : Color.clear;
        sprite.color = currColor;        
    }

    private void ShowURLTextAbove(bool boolean)
    {
        if (transform.childCount != 0)
        {
            transform.GetChild(0).gameObject.SetActive(boolean);
        }
    }

    IEnumerator HighlightCue(Transform squareTransform, float width)
    {
        highlighting = true;
        Transform mask = squareTransform.GetChild(0);
        Vector2 originalMaskPos = mask.position;

        for (float t = 0; t < 1f; t += Time.deltaTime * timeMultiplier)
        {
            mask.position = Vector2.Lerp(originalMaskPos, new Vector2(originalMaskPos.x + width, originalMaskPos.y), t);
            yield return null;
        }
        highlighting = false;
    }
}
