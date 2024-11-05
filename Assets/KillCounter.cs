using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class KillCounter : MonoBehaviour
{
    public TMP_Text killCounterText;  // Reference to the UI Text to display the kill count
    private int killCount = 0;    // Variable to track the number of kills

    void Start()
    {
        UpdateKillCounterText();  // Initialize the kill counter display
    }

    // Method to increment the kill count
    public void IncrementKillCount()
    {
        killCount++;
        UpdateKillCounterText();
    }

    // Update the UI text display
    private void UpdateKillCounterText()
    {
        if (killCounterText != null)
        {
            killCounterText.text = "Kills: " + killCount.ToString();
        }
    }

    // Method to reset the kill count if needed
    public void ResetKillCount()
    {
        killCount = 0;
        UpdateKillCounterText();
    }

    // Optional: Method to retrieve the current kill count
    public int GetKillCount()
    {
        return killCount;
    }
}
