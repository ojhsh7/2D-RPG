using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float startTime;

    public Define.Player SelectedPlayer;
    public string UserID;

    public float PlayerHP = 100f;//ü��
    public float PlayerExp = 1f; //����ġ 

    private GameObject player;

    public int Coin = 0;

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
        startTime = Time.time;
    }

    public GameObject SpawnPlayer(Transform spawnPos)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Characters/" + SelectedPlayer.ToString());
        GameObject player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);

        return player;
    }

    public void DisplayPlayTime()
    {
        // �÷��� �ð��� ����մϴ�.
        float playTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(playTime / 60F);
        int seconds = Mathf.FloorToInt(playTime % 60F);

        // �÷��� �ð��� �α׷� ����մϴ�.
        Debug.Log($"You have played the game for {minutes} minutes and {seconds} seconds.");
    }

}
