using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // Window
    public GameObject window;
    //Indicator
    public GameObject indicator;
    // Text component
    public TMP_Text dialogueText;
    // Dialogues list
    public List<string> dialogues;
    // Writing speed
    public float writingSpeed;
    // Index on Dialogue
    private int index;
    // Character index
    private int charIndex;
    // Started boolean
    private bool started = false;
    // Wait for next boolean
    private bool waitForNext = false;

    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }

    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }
    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    // Start dialogue
    public void StartDialogue()
    {
        if (started)
            return;
        
        // Boolean to indicate that we have started
        started = true;
        // Show the window
        ToggleWindow(true);
        // Hide the indicator
        ToggleIndicator(false);
        // Start with first dialogue
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
        // Start index at zero
        index = i;
        // Reset the character index
        charIndex = 0;
        // Clear the dialogue component text
        dialogueText.text = string.Empty;
        // Start writing
        StopAllCoroutines();
        StartCoroutine(Writing());
    }

    // End dialogue
    public void EndDialogue()
    {
        // Started is disabled
        started = false;
        // waitForNext is disabled
        waitForNext = false;
        // Stop all IEnumerators
        StopAllCoroutines();
        // Hide the window
        ToggleWindow(false);
    }
    // Writing logic
    IEnumerator Writing()
    {
        string currentDialogue = dialogues[index];

        while (charIndex < currentDialogue.Length)
        {
            // Add next character
            dialogueText.text += currentDialogue[charIndex];

            // Move to next character
            charIndex++;

            // Wait before next letter
            yield return new WaitForSeconds(writingSpeed);
        }

        // Finished writing the sentence
        waitForNext = true;
    }

    private void Update()
    {
        if (!started)
            return;

        if (waitForNext && InputManager.instance.InteractPressed())
        {
            waitForNext = false;
            index++;
            // Check if we are in the scope of dialogues list
            if (index < dialogues.Count)
            {
                // If so fetch the next dialogue
                GetDialogue(index);
            }
            else 
            {
                // If not end the dialogue process
                ToggleIndicator(true);
                EndDialogue();
            }
        }
    }
}
