using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popupmsgmanager : MonoBehaviour
{
    public static Popupmsgmanager Instance;
    public Text popupText;
    public float displayTime = 3f;

    private GameObject Panel;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Panel = popupText.transform.parent.parent.parent.gameObject;
    }

    public void ShowPopupMessage(string message)
    {
        Panel.SetActive(true);
        popupText.text = message;
        StartCoroutine(HideMessageAfterDelay());
    }

    IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        popupText.text = "";
        Panel.SetActive(false);
    }
}
