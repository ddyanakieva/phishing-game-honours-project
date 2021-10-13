using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    public CueSelectionMethod cueSelectionScript;   
    public List<RaycastResult> raycastResults;
    public GameObject emailContent;

    private GraphicRaycaster graphicsRaycaster;
    private PointerEventData pointerEventData;

    protected void Start()
    {
        graphicsRaycaster = this.GetComponent<GraphicRaycaster>();
        //Create list to receive all results
        raycastResults = new List<RaycastResult>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //Create the PointerEventData with null for the EventSystem
            pointerEventData = new PointerEventData(null);
            //Set required parameters, in this case, mouse position
            pointerEventData.position = Input.mousePosition;

            graphicsRaycaster.Raycast(pointerEventData, raycastResults);            

            foreach (var hit in raycastResults)
            {               
                if (hit.gameObject.tag == "EmailElements")
                {
                    TextMeshProUGUI tmpUGUI =  hit.gameObject.GetComponent<TextMeshProUGUI>();
                    int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpUGUI, pointerEventData.position, Camera.main);        

                    if (linkIndex > -1)
                    {
                        TMP_LinkInfo linkInfo = tmpUGUI.textInfo.linkInfo[linkIndex];      
                        cueSelectionScript.SelectCue(linkInfo.GetLinkID());
                    }
                }
                else if (hit.gameObject.tag == "Attachment")
                {
                    var rectTransform = hit.gameObject.GetComponent<RectTransform>();
                    Vector3[] v = new Vector3[4];
                    rectTransform.GetWorldCorners(v);
                    float leftX = v[0].x, rightX = v[0].x;
                    float bottomY = v[0].y, topY = v[0].y;
                    foreach (var pos in v)
                    {
                        leftX = Mathf.Min(leftX, pos.x);
                        rightX = Mathf.Max(rightX, pos.x);
                        bottomY = Mathf.Min(bottomY, pos.y);
                        topY = Mathf.Max(topY, pos.y);
                    }
                    float width = leftX - rightX;
                    //float height = bottomY - topY;
                    //cueSelectionScript.SelectCue(-width+0.3f, -0.6f, new Vector3(leftX-0.15f, bottomY+0.4f, 0f));
                    //cueSelectionScript.SelectCue(leftX + 0.5f, rightX + 0.5f, bottomY + 0.5f, topY + 0.5f, bottomY + 0.5f);
                }
            }
            // Clear list
            raycastResults.Clear();
        }
    }
}
