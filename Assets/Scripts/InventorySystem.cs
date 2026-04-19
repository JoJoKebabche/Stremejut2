using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    // List of items picked up 
    public List<GameObject> items = new List<GameObject>();
    // Flag indicates if inventory is open or not 
    public bool isOpen = false;

    int selectedItemId = -1;
    // Inventory System Window 
    [Header("UI Items Section")]
    public GameObject ui_Window;
    public Image[] items_Images;
    [Header("UI Items Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public Text description_Title;
    public Text description_Text;

    private void Update()
    {
        if (InputManager.instance.InventoryPressed())
        {
            ToggleInventory();
        }
    }
    void ToggleInventory()
    {
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);
        Update_UI();
    }
    public void PickUp(GameObject item)
    {
        items.Add(item);
        Update_UI();
    }
    public bool CanPickUp()
    {
        if (items.Count >= items_Images.Length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // Refresh the UI elements in the inventory window void 
    public void Update_UI()
    {
        // For each item in the items list
        // Show it in the respective slot in the items_Images 
        for (int i = 0; i < items_Images.Length; i++)
        {
            if (i < items.Count)
            {
                items_Images[i].sprite =
                    items[i].GetComponent<SpriteRenderer>().sprite;

                items_Images[i].color = Color.white;
            }
            else
            {
                items_Images[i].sprite = null;
                items_Images[i].color = new Color(1, 1, 1, 0); // hides icons
            }
        }
    }
    // Hide all the items ui images
    void HideAll()
    {
        foreach (var i in items_Images)
        {
            i.gameObject.SetActive(false);
        }
    }
    public void ShowDescription(int id)
    {

        selectedItemId = id;
        // Set the Image 
        description_Image.sprite = items_Images[id].sprite;
        // Set the text 
        description_Title.text = items[id].name;
        // Set the description
        description_Text.text = items[id].GetComponent<Item>().descriptionText;

        // Show the elements 
        description_Image.gameObject.SetActive(true); description_Title.gameObject.SetActive(true);
        description_Text.gameObject.SetActive(true);
    }
    public void HideDescription()
    {
        description_Image.gameObject.SetActive(false); description_Title.gameObject.SetActive(false);
        description_Text.gameObject.SetActive(false);
    }
    public void Consume()
    {
        if (selectedItemId < 0 || selectedItemId >= items.Count)
            return;

        Item item = items[selectedItemId].GetComponent<Item>();
        if (item.itemType == Item.ItemType.Consumables)
        {
            Debug.Log($"CONSUMED {items[selectedItemId].name}");
            // Invoke the consume custom event
            items[selectedItemId].GetComponent<Item>().consumeEvent.Invoke();
            // Delete the item once in very tiny time
            Destroy(items[selectedItemId], 0.1f);
            // Clear the Item from the List

            items.RemoveAt(selectedItemId);

            selectedItemId = -1; // Clear the selection
            // Update the UI
            HideDescription();
            Update_UI();
        }
    }
}