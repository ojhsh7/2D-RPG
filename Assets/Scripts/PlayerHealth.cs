using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject deathTextObject; // �׾����ϴ� �ؽ�Ʈ ������Ʈ
    public float deathMessageDuration = 2.0f; // �׾����ϴ� �޽��� ǥ�� �ð�

    void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.PlayerHP = currentHealth;
        deathTextObject.SetActive(false); // �׾����ϴ� �ؽ�Ʈ ��Ȱ��ȭ
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        GameManager.Instance.PlayerHP = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player health reached zero!");
            Die(); // ��� ó�� �Լ� ȣ��
        }
    }


    void Die()
    {
        // �÷��̾� ��� ó��
        Debug.Log("Player died!");
        StartCoroutine(ShowDeathMessageAndLoadScene());
    }

    IEnumerator ShowDeathMessageAndLoadScene()
    {
        // �׾����ϴ� �ؽ�Ʈ Ȱ��ȭ
        deathTextObject.SetActive(true);

        // ������ �ð� ���� ���
        yield return new WaitForSeconds(deathMessageDuration);

        // ĳ���� ���� ������ �̵�
        SceneManager.LoadScene("CharacterSelectionScene"); // �� �̸��� ���� ĳ���� ���� �� �̸����� �ٲټ���
    }
}
