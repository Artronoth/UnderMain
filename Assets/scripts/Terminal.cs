//    #####  TERMINAL SCRIPT  #####
//    A SCRIPT FOR MAKING UNDERMAIN PLAYABLE OUTSIDE OF THE EDITOR
//    By Mitchel Smith (with the help of Molly McCollum)
//
//    #####  HOW TO USE THIS SCRIPT  #####
// 1. To print a new line to the screen, use the AddLine(string) function and pass through your dialogue line.
//    Make sure not to add a line break as the script does it automatically. For example:
//        terminal.AddLine("You have now entered the room. What will you do?");
// 2. To clear the terminal, use the ClearTerminal() function. This will clear all the main text from the
//    screen. For example:
//        terminal.ClearTerminal();
// 3. To change the control scheme displayed at the bottom of the screen, use the UpdateControlScheme(string)
//    function and pass through the new controls. Use this example as a reference for the recommended format:
//        A=Jump, B=Crouch, X=Reload/Interact, Y=Switch Weapons

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public TextMeshProUGUI tmpMainText;
    public TextMeshProUGUI tmpBottomLeftText;
    public TextMeshProUGUI tmpBottomRightText;
    public List<string> lines = new List<string>();
    public bool isTimerActive = true;
    [SerializeField] private int maxLines = 16;
    [SerializeField] private int maxCharsPerLine = 86;

    private void Start()
    {
        StartCoroutine(BeginTimer());
    }
    
    public void AddLine(string newLine)
    {
        int lineAmounts = DetermineLinesNeeded(newLine.Length);
        if (lineAmounts > 1)
        {
            List<string> newMultiLines = new List<string>();
            // Thank you Molly for helping with this
            for (int i = 0; i < Mathf.Ceil(newLine.Length/maxCharsPerLine); i++)
            {
                newMultiLines.Add(newLine.Substring(i * maxCharsPerLine, maxCharsPerLine) + "\n");
            }
            foreach (string str in newMultiLines)
            {
                lines.Add(str);
            }
            newMultiLines.Clear();
        }
        else
        {
            string newNewLine = newLine + "\n";
            lines.Add(newNewLine);
        }
        if (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
            tmpMainText.text = "";
            for (int i = 0; i < lines.Count; i++)
            {
                tmpMainText.text += lines[i];
            }
        }
        else tmpMainText.text += lines[lines.Count - 1];

        // Ensure that the ScrollRect content scrolls to the bottom
        ScrollToBottom();
    }

    // This will ensure the content scrolls to the bottom (making the new line visible)
    void ScrollToBottom()
    {
        // Check if the ScrollRect component is attached to the parent
        var scrollRect = tmpMainText.GetComponentInParent<ScrollRect>();
        if (scrollRect)
        {
            // Scroll to the bottom using the normalized position
            scrollRect.verticalNormalizedPosition = 50f; // 0 means bottom
        }
    }

    private int DetermineLinesNeeded(int lineLength)
    {
        var linesNeeded = (int)Mathf.Ceil(lineLength / maxCharsPerLine);
        return linesNeeded;
    }

    public void ClearTerminal() 
    {
        lines.Clear();
        tmpMainText.text = "";
    }

    public void UpdateControlScheme(string newScheme)
    {
        tmpBottomLeftText.text = newScheme;
    }

    private IEnumerator BeginTimer()
    {
        float seconds = 0;
        float minutes = 0;
        while (isTimerActive)
        {
            seconds += Time.deltaTime;
            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }
            tmpBottomRightText.text = "Undermain [" + minutes + ":" + Mathf.Floor(seconds).ToString("00") + "]";
            yield return null;
        }
    }
}
