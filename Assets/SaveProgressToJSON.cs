using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

public class SaveProgressToJSON : MonoBehaviour
{

    public EmailData _EmailData;
    public Cues _Cues;
    private Dictionary<string, List<EmailData>> allDataProgress;
    private string stringEmailData, stringCueData;

    [DllImport("__Internal")]
    private static extern void SaveGameData(string stringEmailData, string stringCueData);

    public void Start()
    {
        allDataProgress = new Dictionary<string, List<EmailData>>() { { "data_output", new List<EmailData>() } };
    }
    // save incoming data about the emails a user has gone through
    // write in json
    public void SaveToJSON()
    {
        stringEmailData = JsonUtility.ToJson(_EmailData);
        stringCueData = JsonUtility.ToJson(_Cues);
        #if (UNITY_WEBGL == true && UNITY_EDITOR == false)
              SaveGameData (stringEmailData, stringCueData);
        #endif

        //File.WriteAllText(Application.dataPath + "/research_email_output.json", stringEmailData);
        //File.WriteAllText(Application.dataPath + "/research_cue_output.json", stringCueData);
        //Debug.Log("JSON done");
    }


    public void WriteData(int email_id, string is_classfied_phishing, List<string> selectedCues)
    {
        Data email_data = new Data();
        Cue cue_data = new Cue();


        email_data.email_id = email_id;
        cue_data.email_id = email_id;

        email_data.is_classified_phishing = is_classfied_phishing;

        foreach (string id in selectedCues)
        {
            cue_data.selectedCues.Add(id);
        }

        _EmailData.email_classification.Add(email_data);
        _Cues.cue_selection.Add(cue_data);
    }
}

