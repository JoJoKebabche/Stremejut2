using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    void Awake()
    {
        instance = this;
        LoadKeys();
    }

    public void Start()
    {
        UpdateAllUI();
    }

     // BUTTON TEXT
    [SerializeField] TMP_Text crouchText;
    [SerializeField] TMP_Text jumpText;
    [SerializeField] TMP_Text shootText;
    [SerializeField] TMP_Text interactText;
    [SerializeField] TMP_Text inventoryText;
    [SerializeField] TMP_Text moveText;

    // DEFAULT KEYS
    public KeyCode moveLeft1 = KeyCode.A;
    public KeyCode moveRight1 = KeyCode.D;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode shootKey = KeyCode.R;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode runKey = KeyCode.LeftShift;

    // INPUT GETTERS

    public float GetHorizontal()
    {
        float value = 0;

        if (Input.GetKey(moveLeft1))
            value = -1;

        if (Input.GetKey(moveRight1))
            value = 1;

        return value;
    }

    public bool JumpPressed()
    {
        return Input.GetKeyDown(jumpKey);
    }

    public bool ShootPressed()
    {
        return Input.GetKeyDown(shootKey);
    }

    public bool CrouchHeld()
    {
        return Input.GetKey(crouchKey);
    }

    public bool InteractPressed()
    {
        return Input.GetKeyDown(interactKey);
    }
    public bool InventoryPressed()
    {
        return Input.GetKeyDown(inventoryKey);
    }

    public bool RunHeld()
    {
        return Input.GetKey(runKey);
    }

    // ===== REBIND SYSTEM =====

    public void RebindKeyWithUI(string actionName, TMP_Text buttonText, Action<KeyCode> setKey)
    {
        StartCoroutine(RebindRoutine(actionName, buttonText, setKey));
    }

    IEnumerator RebindRoutine(string actionName, TMP_Text buttonText, Action<KeyCode> setKey)
    {
        // Show waiting text
        buttonText.text = "Press to Rebind";

        // Wait for key
        while (true)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    // Set new key
                    setKey(key);

                    // Save keys
                    SaveKeys();

                    // Update UI text
                    buttonText.text = actionName + " - " + key.ToString();

                    yield break;
                }
            }
            yield return null;
        }
    }

    public void RebindCrouch(TMP_Text text)
    {
        RebindKeyWithUI("Crouch", text, key => crouchKey = key);
    }

    public void RebindJump(TMP_Text text)
    {
        RebindKeyWithUI("Jump", text, key => jumpKey = key);
    }

    public void RebindShoot(TMP_Text text)
    {
        RebindKeyWithUI("Shoot", text, key => shootKey = key);
    }

    public void RebindInteract(TMP_Text text)
    {
        RebindKeyWithUI("Interact", text, key => interactKey = key);
    }

    public void RebindInventory(TMP_Text text)
    {
        RebindKeyWithUI("Inventory", text, key => inventoryKey = key);
    }

    public void RebindCrouchButton()
    {
        RebindCrouch(crouchText);
    }

    public void RebindJumpButton()
    {
        RebindJump(jumpText);
    }

    public void RebindShootButton()
    {
        RebindShoot(shootText);
    }

    public void RebindInteractButton()
    {
        RebindInteract(interactText);
    }

    public void RebindInventoryButton()
    {
        RebindInventory(inventoryText);
    }

    public void RebindMovementButton()
    {
        StartCoroutine(RebindMovementRoutine());
    }

    IEnumerator RebindMovementRoutine()
    {
        moveText.text = "Press LEFT key...";
        KeyCode newLeftKey = KeyCode.None;
        // Wait for left key
        while (newLeftKey == KeyCode.None)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    newLeftKey = key;
                    moveLeft1 = key;
                    break;
                }
            }
            yield return null;
        }

        moveText.text = "Press RIGHT key...";
        KeyCode newRightKey = KeyCode.None;
        // Wait for right key
        while (newRightKey == KeyCode.None)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    newRightKey = key;
                    moveRight1 = key;
                    break;
                }
            }
            yield return null;
        }

        // Save and update UI
        SaveKeys();
        UpdateAllUI();
    }

    // SAVE / LOAD

    void SaveKeys()
    {
        PlayerPrefs.SetInt("jump", (int)jumpKey);
        PlayerPrefs.SetInt("shoot", (int)shootKey);
        PlayerPrefs.SetInt("crouch", (int)crouchKey);
        PlayerPrefs.SetInt("interact", (int)interactKey);
        PlayerPrefs.SetInt("left", (int)moveLeft1);
        PlayerPrefs.SetInt("right", (int)moveRight1);
        PlayerPrefs.SetInt("inventory", (int)inventoryKey);
    }

    void LoadKeys()
    {
        jumpKey = (KeyCode)PlayerPrefs.GetInt("jump", (int)KeyCode.Space);
        shootKey = (KeyCode)PlayerPrefs.GetInt("shoot", (int)KeyCode.R);
        crouchKey = (KeyCode)PlayerPrefs.GetInt("crouch", (int)KeyCode.LeftControl);
        interactKey = (KeyCode)PlayerPrefs.GetInt("interact", (int)KeyCode.E);
        moveLeft1 = (KeyCode)PlayerPrefs.GetInt("left", (int)KeyCode.A);
        moveRight1 = (KeyCode)PlayerPrefs.GetInt("right", (int)KeyCode.D);
        inventoryKey = (KeyCode)PlayerPrefs.GetInt("inventory", (int)KeyCode.I);
    }

    public void UpdateAllUI()
    {
        crouchText.text = "Crouch - " + crouchKey;
        jumpText.text = "Jump - " + jumpKey;
        shootText.text = "Shoot - " + shootKey;
        interactText.text = "Interact - " + interactKey;
        inventoryText.text = "Inventory - " + inventoryKey;
        moveText.text = "Movement - " + moveLeft1 + " " + moveRight1;
    }
}
