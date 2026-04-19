using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public Text nameText;
    public SpriteRenderer artworkSprite;

    private int selectedOption = 0;

    void Start()
    {

        Debug.Log("RAW PREF VALUE: " + PlayerPrefs.GetInt("selectedOption", 999));

        Load();

        if (selectedOption == -1)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
    }

    public void NextOption()
    {
        Debug.Log("NEXT CALLED");

        selectedOption++;

        if (selectedOption >= characterDB.characterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
    }

    public void BackOption()
    {
        selectedOption--;

        if (selectedOption < 0)
        {
            selectedOption = characterDB.characterCount - 1;
        }

        UpdateCharacter(selectedOption);
    }

    private void UpdateCharacter(int selectedOption)
    {
        Debug.Log("UPDATE CHARACTER: " + selectedOption);
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;
    }

    public void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption", -1);

        Debug.Log("LOADED: " + selectedOption);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("selectedOption",  selectedOption);
        PlayerPrefs.Save();
        Debug.Log("SAVED: " + selectedOption);
    }

    public void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
        PlayerPrefs.Save();
        Debug.Log("SAVED: " + selectedOption);
    }
}
