using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardSystem : MonoBehaviour
{
    public float pointsPerCue = 10f;
    public float pointsForCorrectChoice = 50f;
    public float pointsForMissedRound = 70f;
    public Slider progressBar;
    public EmailsLogic emailsLogic;
    public TextMeshProUGUI pointsUIText;
    public Color colorWrongAnswer, colorRightAnswer;

    private Color colorToSet;
    private bool userInput = true;
    public float userPoints = 100;
    public float userPointsBefore = 100;
    public int phishEmailsCorrect = 0, genuineEmailsCorrect = 0, totalEmailsCorrect = 0;

    private void Update()
    {
        if (!userInput)
        {
            emailsLogic.MarkEmpty();
            userInput = true;
        }
    }

    private IEnumerator StartTimer()
    {
        for(float t = 0; t < 60f; t+=Time.deltaTime)
        {
            progressBar.value += 0.00012f;
            yield return null;
        }
        userInput = false;
    }
    // timer should reset after every email
    public void ResetTimer()
    {
        progressBar.value = 0;
        StartCoroutine(StartTimer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        progressBar.value = 0;
        userInput = true;
    }
    
    public void CountPoints(bool userAnswered, string userChoice, bool isPhishing, int numberOfUserCues)
    {
        if (!userAnswered) {
            userPoints -= pointsForMissedRound;
            colorToSet = colorWrongAnswer;
            userPoints = Mathf.RoundToInt(userPoints);
            StartCoroutine(SetPointsAndColor());
            return;
        }
        // if user marked the email correctly
        if (userChoice.Equals(isPhishing.ToString()))
        {
            colorToSet = colorRightAnswer;
            totalEmailsCorrect++;


            // if the email is phishing then give full points & points per correct cue
            if (isPhishing)
            {
                userPoints += pointsForCorrectChoice + numberOfUserCues * pointsPerCue;
                phishEmailsCorrect++;
            }
            // if the email is genuine give full points
            else
            {
                userPoints += pointsForCorrectChoice;
                genuineEmailsCorrect++;
            }
        }
        // if user did not mark the email correctly
        else
        {
            colorToSet = colorWrongAnswer;

            // if the email was phishing deduct more points
            if (isPhishing)
            {
                userPoints -= pointsForMissedRound;
            }
            // if the email was genuine deduct 2/3 of points
            else
            {
                userPoints -= 2 * pointsForCorrectChoice / 3;
            }
        }
        // make a IEnumerator to animate add/substract points 
        // color green/red respectively if less than it was before
        userPoints = Mathf.RoundToInt(userPoints);
        StartCoroutine(SetPointsAndColor());
    }

    IEnumerator SetPointsAndColor()
    {
        pointsUIText.color = colorToSet;
        int multiplier;
        if (userPointsBefore < userPoints) multiplier = 1;
        else multiplier = -1;
        float points = userPointsBefore;
        for (float t = 0; t < 20f; t += Time.deltaTime)
        {
            points += multiplier;
            if (points == userPoints) break;

            pointsUIText.text = points.ToString();
            yield return null;
        }
        userPointsBefore = userPoints;
        yield return new WaitForSeconds(0.5f);
        pointsUIText.color = Color.black;
    }
}
