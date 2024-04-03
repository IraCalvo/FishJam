using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed at which the bubble moves
    public float destroyHeight = 15f; // Height at which the bubble is destroyed
    public GameObject bubbleObject; // Reference to the GameObject containing the bubble elements

    private bool flipX = false; // Flag to control flipping of the bubble

    void Start()
    {
        // Ensure bubbleObject is assigned
        if (bubbleObject == null)
        {
            Debug.LogError("Bubble object reference is not set!");
            enabled = false; // Disable the script
        }
    }

    void Update()
    {
        // Move the bubble upwards
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Check if the bubble has reached the destroy height
        if (transform.position.y >= destroyHeight)
        {
            Destroy(gameObject);
        }

        // Alternate flipping the bubble on the x-axis
        if (Time.frameCount % 60 == 0) // Change flip every second (adjust as needed)
        {
            flipX = !flipX;
            bubbleObject.transform.localScale = new Vector3(flipX ? -1 : 1, 1, 1);
        }
    }
}