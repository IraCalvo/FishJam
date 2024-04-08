using UnityEngine;
using TMPro;

public class ClockUpdater : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private int currentMinute = 0;
    private int currentSecond = 0;
    private float timer = 0f;
    public float secondsPerGameMinute = 1f; // Change this value to adjust the speed of the in-game clock

    void Start()
    {
        // Initialize the in-game time
        UpdateClock();
    }

    void Update()
    {
        // Update the in-game time every frame
        timer += Time.deltaTime;

        // Check if a game minute has passed
        if (timer >= secondsPerGameMinute)
        {
            currentSecond++;
            if (currentSecond >= 60)
            {
                currentSecond = 0;
                currentMinute++;
            }

            if (currentMinute >= 60)
                currentMinute = 0;

            UpdateClock();
            timer = 0f; // Reset the timer
        }
    }

    void UpdateClock()
    {
        // Format the time as MM:SS
        string formattedTime = currentMinute.ToString("00") + ":" + currentSecond.ToString("00");

        // Update the TextMeshPro text
        textMeshPro.text = formattedTime;
    }
}
