using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterStat
{
    public float HP = 100f; // 체력
    public float MP = 100f;
    public float Exp = 1f;  // 경험치 
    public float Def = 1f;
    public int Level = 1;
    public int Coin = 0;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Define.Player SelectedPlayer;
    public string UserID;
    public CharacterStat PlayerStat = new CharacterStat();
    [HideInInspector]
    public GameObject player;


    public Text cooldownText; // 스킬 쿨타임 텍스트 UI 요소

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

    public GameObject SpawnPlayer(Transform spawnPos)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Characters/" + SelectedPlayer.ToString());
        player = Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);

        return player;
    }

    public void UpdateCooldownText(float cooldownTime)
    {
        if (cooldownTime > 0)
        {
            cooldownText.text = "스킬 쿨타임: " + cooldownTime.ToString("F1") + "초";
        }
        else
        {
            cooldownText.text = "";
        }
    }
    public Character Character
    {
        get { return player.GetComponent<Character>(); }
    }
    public Attack CharacterAttack
    {
        get { return Character.AttackObj.GetComponent<Attack>(); }
    }
}
