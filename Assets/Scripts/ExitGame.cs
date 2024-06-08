using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton; // Exit ��ư ����
    public GameObject messagePanel; // �޽��� �г� ����
    public Text messageText; // �޽��� �ؽ�Ʈ ����

    void Start()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
        messagePanel.SetActive(false); // ���� �� �޽��� �г� ��Ȱ��ȭ
    }

    void OnExitButtonClick()
    {
        messageText.text = "������ �����մϴ�."; // �޽��� ����
        messagePanel.SetActive(true); // �޽��� �г� Ȱ��ȭ
        StartCoroutine(ExitAndLoadStartScene());
    }

    IEnumerator ExitAndLoadStartScene()
    {
        yield return new WaitForSeconds(2); // 2�� ���
        SceneManager.LoadScene("StartScene"); // StartScene���� ��ȯ
    }
}

