using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string CharacterName;
    public string UserID;

    public float PlayerHP = 100f;//ü��
    public float PlayerExp = 1f; //����ġ 

    private void Awake()
    {
        if (Instance == null)
        {
        Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        UserID = PlayerPrefs.GetString("ID");
    }
}