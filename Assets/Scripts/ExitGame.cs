using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton; // Exit ��ư ����
    public Button backButton; // �ǵ��ư��� ��ư ����
    public Button yesButton; // �� ��ư ����
    public Button noButton; // �ƴϿ� ��ư ����
    public GameObject messagePanel; // �޽��� �г� ����
    public GameObject confirmPanel; // Ȯ�� �г� ����
    public Text messageText; // �޽��� �ؽ�Ʈ ����
    public Text confirmText; // Ȯ�� �ؽ�Ʈ ����

    private int previousSceneIndex; // ���� ���� �ε��� ���� ����
    private bool isExitAction; // ���� ������ ���� �������� ����

    void Start()
    {
        // UI ��Ұ� ��� ����Ǿ����� Ȯ��
        if (exitButton == null || backButton == null || yesButton == null || noButton == null ||
            messagePanel == null || confirmPanel == null || messageText == null || confirmText == null)
        {
            Debug.LogError("UI ��Ұ� ������� �ʾҽ��ϴ�. ����Ƽ �����Ϳ��� ��� ��Ҹ� Ȯ���ϼ���.");
            return;
        }

        // ��ư Ŭ�� �̺�Ʈ ������ ����
        exitButton.onClick.AddListener(OnExitButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);

        // �г� �ʱ� ��Ȱ��ȭ
        messagePanel.SetActive(false);
        confirmPanel.SetActive(false);

        // ���� ���� �ε��� ���� (���� ���� ����Ͽ� ���� �ʿ�)
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }

    void OnExitButtonClick()
    {
        confirmText.text = "������ �����Ͻðڽ��ϱ�?"; // Ȯ�� �޽��� ����
        confirmPanel.SetActive(true); // Ȯ�� �г� Ȱ��ȭ
        isExitAction = true; // ���� �������� ����
    }

    void OnBackButtonClick()
    {
        confirmText.text = "���� ȭ������ ���ư��ðڽ��ϱ�?"; // Ȯ�� �޽��� ����
        confirmPanel.SetActive(true); // Ȯ�� �г� Ȱ��ȭ
        isExitAction = false; // ���� ���� �ƴ����� ����
    }

    void OnYesButtonClick()
    {
        if (isExitAction)
        {
            messageText.text = "������ �����մϴ�."; // ���� �޽��� ����
            messagePanel.SetActive(true); // �޽��� �г� Ȱ��ȭ
            StartCoroutine(ExitAndLoadStartScene());
        }
        else
        {
            messageText.text = "���� ȭ������ ���ư��ϴ�."; // ���� ȭ�� �޽��� ����
            messagePanel.SetActive(true); // �޽��� �г� Ȱ��ȭ
            StartCoroutine(BackAndLoadPreviousScene());
        }
        confirmPanel.SetActive(false); // Ȯ�� �г� ��Ȱ��ȭ
    }

    void OnNoButtonClick()
    {
        confirmPanel.SetActive(false); // Ȯ�� �г� ��Ȱ��ȭ
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
