
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerUI : MonoBehaviour
{
    public Image CharacterImg;
    public Text IdText;
    public Text LvText;

    public Slider HpSlider;
    public Slider MPSlider;
    public Slider ExpSlider;
    private GameObject player;
    

    void Start()
    {
        IdText.text = GameManager.Instance.UserID;
        GameObject spawnPos = GameObject.FindGameObjectWithTag("initPos");
        player = GameManager.Instance.SpawnPlayer(spawnPos.transform);  
    }
    void Update()
    {
        display();
    }

    private void display()
    {
        CharacterImg.sprite = player.GetComponent<SpriteRenderer>().sprite;
        HpSlider.value = GameManager.Instance.PlayerStat.HP;
        MPSlider.value = GameManager.Instance.PlayerStat.MP;
        ExpSlider.value = GameManager.Instance.PlayerStat.Exp;
        LvText.text = "Lv :  " + GameManager.Instance.PlayerStat.Level;
    }

}
