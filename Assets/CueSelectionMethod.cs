using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CueSelectionMethod : EmailsLogic
{
    private List<GameObject> highlighterObjects;

    public new void Initialize()
    {
        selectedCues = new List<string>();
        highlighterObjects = new List<GameObject>();
        foreach (var tmp in tmpUGUIList) CreateHighlighterObjects(tmp);
    }

    public void SelectCue(string cueID)
    {
        // if clicked cue exists in dictionary/list then remove

        if (selectedCues.Contains(cueID))
        {
            selectedCues.Remove(cueID);
        }
        else
        {
            selectedCues.Add(cueID);
        }
    }

    private void CreateHighlighterObjects(TextMeshProUGUI tmpUGUI)
    {
        for (int i = 0; i < tmpUGUI.textInfo.linkCount; i++)
        {
            TMP_LinkInfo linkInfo = tmpUGUI.textInfo.linkInfo[i];
            string linkID = linkInfo.GetLinkID();

            TMP_CharacterInfo firstChar = tmpUGUI.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex];
            TMP_CharacterInfo lastChar = tmpUGUI.textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength - 1];
                     
            Vector2 textBottomLeft = tmpUGUI.transform.TransformPoint(new Vector2(firstChar.bottomLeft.x, firstChar.baseLine));
            Vector2 textBottomRight = tmpUGUI.transform.TransformPoint(lastChar.bottomRight);            

            float width = textBottomRight.x - textBottomLeft.x;
            float height = 3.5f * tmpUGUI.fontScale;

            GameObject square = Instantiate(cueObject);
            square.name = "highlighter " + linkID;
            highlighterObjects.Add(square);

            // generate
            square.transform.localScale = new Vector3(Mathf.Abs(width), Mathf.Abs(height), 0f);
            if (linkID.Contains("link")) CreateHoverOverText(linkID,square);
            square.transform.position = new Vector3(textBottomLeft.x + width / 2, textBottomLeft.y + height / 4, 0f);
            square.AddComponent<BoxCollider2D>().size = new Vector2(1.2f, 1.3f);

            // give Highlighter class and initialize
            square.AddComponent<Highlighter>().Initialize();
        }

        // pass number of cues to reward system
    }

    // Create the hoverable URL links
    private void CreateHoverOverText(string linkID, GameObject square)
    {
        GameObject linkText = Instantiate(linkTextObject);
        linkText.name = "link " + linkID;
        string text = allEmails[currentEmail].linkURL;
        linkText.GetComponent<TextMeshPro>().text = text;
        linkText.transform.parent = square.transform;
        linkText.GetComponentInChildren<SpriteRenderer>().size = new Vector2(text.Length*0.16f, 0.48f);
    }

    public void ClearSelectedCues()
    {
        raycasterScript.raycastResults.Clear();
        selectedCues.Clear();

        // destroy existing gameobjects
        int count = highlighterObjects.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(highlighterObjects[i]);
        }
        // clear list of existing gameobjects
        highlighterObjects.Clear();
    }

    public void InitializeNewHighlighterObjects()
    {
        // get the new text from tmpuguis
        foreach (TextMeshProUGUI tmp in tmpUGUIList)
        {
            CreateHighlighterObjects(tmp);
        }
    }

}
