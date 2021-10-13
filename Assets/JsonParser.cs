using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// need to implement cues
[System.Serializable]
public class Email
{
    public int id;
    public bool is_phishing;
    public string name;
    public string email_address;
    public string subject;
    public string content; // email body
    public string linkURL;
    public string attachment;
    // add cues
}

[System.Serializable]
public class Emails
{
    public Email[] emails;
}

[System.Serializable]
public class Cue
{
    public int email_id;
    public List<string> selectedCues = new List<string>();
}

[System.Serializable]
public class Cues
{
    public List<Cue> cue_selection = new List<Cue>();
}

[System.Serializable]
public class EmailData
{
    public List<Data> email_classification = new List<Data>();
}

[System.Serializable]
public class Data
{
    public int email_id;
    public string is_classified_phishing;
    //public List<string> selectedCues = new List<string>();
}
