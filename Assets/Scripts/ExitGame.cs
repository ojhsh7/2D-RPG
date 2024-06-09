using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton; // Exit ��ư ����
    public Button backButton; // �ǵ��ư��� ��ư ����
    public GameObject messagePanel; // �޽��� �г� ����
    public Text messageText; // �޽��� �ؽ�Ʈ ����

    private int previousSceneIndex; // ���� ���� �ε��� ���� ����

    void Start()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        messagePanel.SetActive(false); // ���� �� �޽��� �г� ��Ȱ��ȭ

        // ���� ���� �ε��� ���� (���� ���� ����Ͽ� ���� �ʿ�)
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }

    void OnExitButtonClick()
    {
        messageText.text = "������ �����մϴ�."; // �޽��� ����
        messagePanel.SetActive(true); // �޽��� �г� Ȱ��ȭ
        StartCoroutine(ExitAndLoadStartScene());
    }

    void OnBackButtonClick()
    {
        messageText.text = "���� ȭ������ ���ư��ϴ�."; // �޽��� ����
        messagePanel.SetActive(true); // �޽��� �г� Ȱ��ȭ
        StartCoroutine(BackAndLoadPreviousScene());
    }

    IEnumerator ExitAndLoadStartScene()
    {
        yield return new WaitForSeconds(2); // 2�� ���
        SceneManager.LoadScene("StartScene"); // StartScene���� ��ȯ
    }

    IEnumerator BackAndLoadPreviousScene()
    {
        yield return new WaitForSeconds(2); // 2�� ���
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex); // ���� ������ ��ȯ
        }
        else
        {
            Debug.LogWarning("���� ���� �������� �ʾҽ��ϴ�.");
        }
    }
}
