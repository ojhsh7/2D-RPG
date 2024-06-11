using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject deathTextObject; // 죽었습니다 텍스트 오브젝트
    public float deathMessageDuration = 2.0f; // 죽었습니다 메시지 표시 시간

    void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.PlayerHP = currentHealth;
        deathTextObject.SetActive(false); // 죽었습니다 텍스트 비활성화
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        GameManager.Instance.PlayerHP = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player health reached zero!");
            Die(); // 사망 처리 함수 호출
        }
    }


    void Die()
    {
        // 플레이어 사망 처리
        Debug.Log("Player died!");
        StartCoroutine(ShowDeathMessageAndLoadScene());
    }

    IEnumerator ShowDeathMessageAndLoadScene()
    {
        // 죽었습니다 텍스트 활성화
        deathTextObject.SetActive(true);

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(deathMessageDuration);

        // 캐릭터 선택 씬으로 이동
        SceneManager.LoadScene("CharacterSelectionScene"); // 씬 이름을 실제 캐릭터 선택 씬 이름으로 바꾸세요
    }
}
