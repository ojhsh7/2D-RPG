using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject SkillExplainUI;
    public Image SkillImage;
    public Text SkillText;

    public Image[] Skills;
    private float SkillSpeed = 6f;

    public void ExplainSkillBtn(int number)
    {
        Debug.Log("ExplainSkillBtn ");
        SkillExplainUI.SetActive(true);
        SkillImage.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;

        switch (GameManager.Instance.SelectedPlayer)
        {
            case Define.Player.Warrior:
                if (number == 0) SkillText.text = "전사의 첫 번째 스킬";
                else if (number == 1) SkillText.text = "전사의 두 번째 스킬";
                else if (number == 2) SkillText.text = "전사의 세 번째 스킬";
                break;
            case Define.Player.Archer:
                if (number == 0) SkillText.text = "궁수의 첫 번째 스킬";
                else if (number == 1) SkillText.text = "궁수의 두 번째 스킬";
                else if (number == 2) SkillText.text = "궁수의 세 번째 스킬";
                break;
            case Define.Player.Mage:
                if (number == 0) SkillText.text = "마법사의 첫 번째 스킬";
                else if (number == 1) SkillText.text = "마법사의 두 번째 스킬";
                else if (number == 2) SkillText.text = "마법사의 세 번째 스킬";
                break;
        }

        Invoke("ExitExplain", 5f);
    }

    private void ExitExplain()
    {
        SkillExplainUI.SetActive(false);
    }

    private void Update()
    {
        SkillUse();
    }

    private void SkillUse()
    {
        if (GameManager.Instance.PlayerStat.Level >= 5)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (Skills[0].fillAmount >= 1)
                {
                    GameManager.Instance.PlayerStat.MP -= 10f;
                    GameManager.Instance.Character.AttackAnimation();

                    // Skill 생성
                    GameObject playerPrefab = Resources.Load<GameObject>("Skill/W_SKILL_0");

                    Quaternion rotation = Quaternion.identity;
                    float speed = SkillSpeed;
                    if (GameManager.Instance.player.transform.localScale.x < 0)
                    {
                        rotation = Quaternion.Euler(0, 180, 0);
                        speed = SkillSpeed * -1;
                    }

                    GameObject obj = Instantiate(playerPrefab, GameManager.Instance.player.transform.position, rotation);
                    obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
                    Destroy(obj, 5f);

                    StartCoroutine(SkillAmount(0));
                }
            }
        }
    }
    IEnumerator SkillAmount(int skillIndex)
    {
        Skills[skillIndex].fillAmount = 0f;
        while (Skills[skillIndex].fillAmount < 1)
        {
            Skills[skillIndex].fillAmount += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        Skills[skillIndex].fillAmount = 1;
    }
}
