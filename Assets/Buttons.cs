using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public EmailsLogic email;
    public RewardSystem timer;
    public Button acceptB, reportB;


    // Buttons should invoke the reward system script 
    // pass necessary information to reward system script
    // get selected cues from cueSelectionMethod

    public void AcceptButton()
    {
        // mark genuine
        timer.StopTimer();
        email.MarkGenuine();
        StartCoroutine(Wait());
    }

    public void ReportButton()
    {
        // mark phishing
        timer.StopTimer();
        email.MarkPhishing();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        acceptB.interactable = false;
        reportB.interactable = false;
        yield return new WaitForSeconds(1);
        acceptB.interactable = true;
        reportB.interactable = true;
    }
}
