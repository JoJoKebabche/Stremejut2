using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    // Detection Point
    public Transform detectionPoint;
    // Detection Radius
    public const float detectionRadius = 0.2f;
    // Detection Layer
    public LayerMask detectionLayer;
    // Cached Trigger Object
    public GameObject detectedObject;
    [Header("Examine Fields")]
    //Examine window object
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamining = false;

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
       Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if (obj == null) 
        {
            detectedObject = null;
            return false;  
        }
        else 
        { 
            detectedObject = obj.gameObject;
            return true;
        }
    }

    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }
    public void PickUpItems(GameObject item)
    {
        FindFirstObjectByType<InventorySystem>().PickUp(item);
    }

    public void ExamineItem(Item item)
    {

        if (isExamining)
        {
            // Hide the examine window
            examineWindow.SetActive(false);
            // Disable the boolean
            isExamining = false;
        }
        else
        {
            // Show item image in the middle
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            // Write description beneath image
            examineText.text = item.descriptionText;
            // Display an examine window
            examineWindow.SetActive(true);
            // Make the boolean true
            isExamining = true;
        }
    }
}
