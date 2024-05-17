using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    [Header("infor")]
    public Text NameTxt;
    public Text Feature;
    public Image CharImage;

    [Header("Character")]
    public GameObject[] Characters; //Warrior, Archer, Mage
    private int charIndex = 0;


    public void SelectCharactBtn(string btnName)
    {
        Characters[charIndex].SetActive(false);
        if (btnName == "Next")
        {
            charIndex++;
            charIndex = charIndex % Characters.Length;
        }
        if (btnName == "Prev")
        {
            charIndex--;
            charIndex = charIndex % Characters.Length;
            charIndex = charIndex < 0 ? charIndex + Characters.Length : charIndex;
        }
        Debug.Log($"CharIndex : {charIndex}");
        Characters[charIndex].SetActive(true);
    }
}
