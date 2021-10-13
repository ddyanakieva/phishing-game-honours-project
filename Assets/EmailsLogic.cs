using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmailsLogic : MonoBehaviour
{
    public RewardSystem rewardSystem;
    public SaveProgressToJSON saveToJSONScript;
    public Raycaster raycasterScript;
    public GameplayLogic gameplayLogicScript;

    public TextMeshProUGUI fromFieldName, fromFieldAddress, subjectField, emailContent, attachmentContainer, companyNames, domainNames;
    public Scrollbar bodyScrollbar;    
    public GameObject cueObject, linkTextObject;

    public List<TextMeshProUGUI> tmpUGUIList;
    public List<string> selectedCues;
    public int totalPhishEmails = 0, totalGenuineEmails = 0;

    protected Dictionary<int, Email> allEmails;
    protected int currentEmail = 0;

    private string companyNamesText, domainNamesText;
    private int allEmailsCount;
    private string isPhishing_userChoice;
    private bool userAnswered = true;
    //private char[] charsToTrim = { '"', '\'' };
    private CueSelectionMethod cueSelectionScript;

    public void Initialize()
    {
        companyNamesText = companyNames.text;
        domainNamesText = domainNames.text;
        allEmails = new Dictionary<int, Email>();
        //phishingEmailsCollection = new Dictionary<int, string>();
        cueSelectionScript = GetComponent<CueSelectionMethod>();
        GetEmails();
        DisplayEmail();
        cueSelectionScript.Initialize();
    }

    private void DisplayEmail()
    {
        // JSON should have all emails organized in how they would be displayed

        Email emailToDisplay = allEmails[currentEmail];

        if (emailToDisplay.id == 9)
        {
            companyNamesText += "\nMcKinsy";
            domainNamesText += "\n@kinsy-estate.com";
            companyNames.text = companyNamesText;
            domainNames.text = domainNamesText;
        }

        subjectField.text = emailToDisplay.subject;
        emailContent.text = emailToDisplay.content;
        fromFieldAddress.text = emailToDisplay.email_address.ToLower();
        fromFieldName.text = emailToDisplay.name;
      
        attachmentContainer.text = emailToDisplay.attachment;


        bodyScrollbar.value = 1f;
        rewardSystem.ResetTimer(); // start/reset the timer

        Canvas.ForceUpdateCanvases();
    }
    private void LoadNextEmail()
    {
        saveToJSONScript.WriteData(allEmails[currentEmail].id, isPhishing_userChoice, cueSelectionScript.selectedCues);
        rewardSystem.CountPoints(userAnswered, isPhishing_userChoice, allEmails[currentEmail].is_phishing, cueSelectionScript.selectedCues.Count);

        currentEmail++;

        if (currentEmail < allEmailsCount)
        {           
            //clear cue selected
            cueSelectionScript.ClearSelectedCues();   
            DisplayEmail();
            cueSelectionScript.InitializeNewHighlighterObjects();
        }
        else
        {
            // Show end screen
            // save data to JSON file
            saveToJSONScript.SaveToJSON();
            cueSelectionScript.ClearSelectedCues();
            gameplayLogicScript.DisplayNextModule();
            //foreach (KeyValuePair<int, bool> entry in phishingEmailsCollection)
            //{
            //    // "entry.Key" will equal "allEmails[entry.Key].id"
            //    // if entry (email) is Phishing 
            //    if (entry.Value)
            //    {
            //        // do something
            //    }
            //}
        }
    }

    // once Marked the email should become unavailable to configure
    // don't allow participants to click email if either button clicked

    // Pass data to SaveProgressToJSON script
    // email ID + type of classification + selected Cues
    public void MarkPhishing()
    {
        if (currentEmail >= allEmailsCount) return;
        userAnswered = true;

        isPhishing_userChoice = "True";
        // advance
        LoadNextEmail();
    }

    public void MarkGenuine()
    {
        if (currentEmail >= allEmailsCount) return;

        isPhishing_userChoice = "False";
        userAnswered = true;
        // advance
        LoadNextEmail();
    }

    public void MarkEmpty()
    {
        if (currentEmail >= allEmailsCount) return;

        isPhishing_userChoice = "null";
        userAnswered = false;
        // advance
        LoadNextEmail();
    }
        
    // store both general & scenario emails
    private void GetEmails()
    {
        int emailsCounter = 0;

        var jsonTextFile = Resources.Load<TextAsset>("emailsJsonOutput");
        string text = jsonTextFile.ToString();
        Emails jsonEmails = JsonUtility.FromJson<Emails>(text);
        foreach (Email email in jsonEmails.emails)
        {
            allEmails.Add(emailsCounter, email);
            emailsCounter++;
            if (email.is_phishing) totalPhishEmails++;
            else totalGenuineEmails++;
        }

        allEmailsCount = emailsCounter;
    }
}
