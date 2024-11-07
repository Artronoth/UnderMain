using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public List<string> lines = new List<string>();
    [SerializeField] private int maxLines = 10;
    private int activeLineAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        tmpText.text = "";
    }
    public void AddLine(string newLine)
    {
        string newNewLine = newLine + "\n";
        lines.Add(newNewLine);
        if (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
            tmpText.text = "";
            for (int i = 0; i > lines.Count; i++)
            {
                tmpText.text += lines[i];
            }
        }
        else tmpText.text += lines[lines.Count - 1];

        // Ensure that the ScrollRect content scrolls to the bottom
        ScrollToBottom();
    }

    // This will ensure the content scrolls to the bottom (making the new line visible)
    void ScrollToBottom()
    {
        // Check if the ScrollRect component is attached to the parent
        var scrollRect = tmpText.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            // Scroll to the bottom using the normalized position
            scrollRect.verticalNormalizedPosition = 0f; // 0 means bottom
        }
    }

    public void ClearTerminal() 
    {
        lines.Clear();
        tmpText.text = "";
    }
}
