using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    public GameObject Dialogue_UI;

    private GameObject playerObj;
    private float distance;

    public GameObject []DialogueTextObj;
    private int dIndex = 0;
    private void Update()
    {
        if (playerObj == null) playerObj = GameManager.Instance.player;
        if (playerObj == null ) return;

        distance = Vector2.Distance(transform.position, playerObj.transform.position);
        Debug.Log($"distance : {distance}");

        if (distance <= 3)
            Dialogue_UI.SetActive(true);
        else 
            Dialogue_UI.SetActive(false);
        

    }
    public void NextBtn(string name)
    {
        DialogueTextObj[dIndex].SetActive(false);
        if (name == "Next")
        {
            if (dIndex < DialogueTextObj.Length - 1) dIndex++;
        }
        else if (name == "Prev")
        {
            if (dIndex > 0) dIndex--;
        }
        DialogueTextObj[dIndex].SetActive(true);
    }
    public void TownBtn()
    {
        SceneManager.LoadScene("TownScene");
    }
}

