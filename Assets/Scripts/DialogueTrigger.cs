using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript;
    private bool playerDetected = false;

    // Detect trigger with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we triggered the player enable playerdetected and show indicator
        if (collision.tag == "Player")
        {
            playerDetected = true;
            dialogueScript.ToggleIndicator(playerDetected);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we lost trigger with the player disable playerdetected and hide indicator
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogueScript.ToggleIndicator(playerDetected);
            dialogueScript.EndDialogue();
        }

    }
    // While detected, if we interact start the dialogue
    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            dialogueScript.StartDialogue();
        }
    }
}
