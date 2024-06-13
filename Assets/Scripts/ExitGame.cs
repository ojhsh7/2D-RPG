using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public GameObject confirmPanel; // Ȯ�� �г� ����
    public Text messageText; // �޽��� �ؽ�Ʈ ����
    public Button yesButton; // �� ��ư ����
    public Button noButton; // �ƴϿ� ��ư ����
    public Button backButton; // �ǵ��ư��� ��ư ����
    public Button homeButton; // Ȩ ��ư ����

    private bool isExitAction; // ���� ������ ���� �������� ����
    private int previousSceneIndex; // ���� ���� �ε��� ���� ����

    void Start()
    {
        // UI ��Ұ� ��� ����Ǿ����� Ȯ��
        if (confirmPanel == null || messageText == null || yesButton == null || noButton == null ||
            backButton == null || homeButton == null)
        {
            Debug.LogError("UI ��Ұ� ������� �ʾҽ��ϴ�. ����Ƽ �����Ϳ��� ��� ��Ҹ� Ȯ���ϼ���.");
            return;
        }

        // ��ư Ŭ�� �̺�Ʈ ������ ����
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);

        // �г� �ʱ� ��Ȱ��ȭ
        confirmPanel.SetActive(false);

        // ���� ���� �ε��� ����
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }

    void OnHomeButtonClick()
    {
        ShowConfirmPanel("������ �����Ͻðڽ��ϱ�?");
        isExitAction = true; // ���� �������� ����
    }

    void OnBackButtonClick()
    {
        ShowConfirmPanel("���� ȭ������ ���ư��ðڽ��ϱ�?");
        isExitAction = false; // ���� ���� �ƴ����� ����
    }

    void ShowConfirmPanel(string message)
    {
        // �޽��� ����
        messageText.text = message;
        // Ȯ�� �г� Ȱ��ȭ
        confirmPanel.SetActive(true);
    }

    void OnYesButtonClick()
    {
        confirmPanel.SetActive(false); // Ȯ�� �г� ��Ȱ��ȭ
        // Unity�� Application.Quit() �޼��带 ����Ͽ� ���� ����
        if (isExitAction)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            // ���� ������ ��ȯ
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

    void OnNoButtonClick()
    {
        confirmPanel.SetActive(false); // Ȯ�� �г� ��Ȱ��ȭ
    }
}
