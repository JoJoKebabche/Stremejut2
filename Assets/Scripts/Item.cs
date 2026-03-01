using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider2D))] 
public class Item : MonoBehaviour
{
    // Interaction Type
    public enum InteractionTypes { NONE, PickUp, Examine}
    public enum ItemType { Static, Consumables }
    [Header("General Attributes")]
    public InteractionTypes interactionType;
    public ItemType itemType;
    public bool stackable = false;
    [Header("Examine")]
    public string descriptionText;
    [Header("Custom Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    // Collider Trigger
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void Interact()
    {
        switch(interactionType)
        {
            case InteractionTypes.PickUp:
                if (!FindFirstObjectByType<InventorySystem>().CanPickUp())
                {
                    Debug.Log("CAN'T PICK UP ANYMORE");
                    return;
                }
                // Add the object to the picked up items list
                FindFirstObjectByType<InteractionSystem>().PickUpItems(gameObject);
                // And then disable the object
                gameObject.SetActive(false);
                Debug.Log("PICK UP");
                AudioManager.instance.SFX("cherry");
                break;
            case InteractionTypes.Examine:
                // call the Examine item in the interaction system
                FindFirstObjectByType<InteractionSystem>().ExamineItem(this);
                Debug.Log("EXAMINE");
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        // Invoke (call) the custom event(s)
        customEvent.Invoke();
    }
}
