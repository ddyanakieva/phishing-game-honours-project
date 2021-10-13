using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayLogic : MonoBehaviour
{
    public GameObject narrativeRound, mainRound, feedbackRound;
    public TextMeshProUGUI scoreText, phishNumText, genuineNumText, totalEmailsNumText, totalPhishNum, totalGenuineNum;
    public EmailsLogic emailsLogicScript;
    public RewardSystem rewardScript;
    public Animator circleAnimation;
    private Dictionary<int,GameObject> modules;
    public int moduleToDisplay = 0;
    private bool loading;

    private void Start()
    {
        modules = new Dictionary<int, GameObject>() { { 0, narrativeRound }, { 1, mainRound }, { 2, feedbackRound } };
        //circleAnimation.SetTrigger("PlayAnimation");
    }
    public void DisplayNextModule()
    {
        circleAnimation.SetTrigger("PlayAnimation");
        StartCoroutine(WaitAndLoad());       
    }

    private IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(1f);
        // disable module before
        if (modules.ContainsKey(moduleToDisplay)) modules[moduleToDisplay].SetActive(false);
        moduleToDisplay++;
        // display next module
        if (modules.ContainsKey(moduleToDisplay)) modules[moduleToDisplay].SetActive(true);
        // initialize module
        if (moduleToDisplay == 1)
        {
            emailsLogicScript.Initialize();
        }
        else if (moduleToDisplay == 2)
        {
            InitializeFeedback();
        }
        yield return null;
    }

    private void InitializeFeedback()
    {
        // final score
        scoreText.text = rewardScript.userPoints.ToString();
        // emails classified correctly
        totalEmailsNumText.text = rewardScript.totalEmailsCorrect.ToString();
        totalPhishNum.text = "/ " + emailsLogicScript.totalPhishEmails.ToString();
        totalGenuineNum.text = "/ " + emailsLogicScript.totalGenuineEmails.ToString();
        phishNumText.text = rewardScript.phishEmailsCorrect < 0 ? "0" : rewardScript.phishEmailsCorrect.ToString();
        genuineNumText.text = rewardScript.genuineEmailsCorrect < 0 ? "0" : rewardScript.genuineEmailsCorrect.ToString();
        // average time per email
    }
}

