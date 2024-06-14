using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public GameObject confirmPanel; // 확인 패널 연결
    public Text messageText; // 메시지 텍스트 연결
    public Button yesButton; // 예 버튼 연결
    public Button noButton; // 아니오 버튼 연결
    public Button backButton; // 되돌아가기 버튼 연결
    public Button homeButton; // 홈 버튼 연결

    private bool isExitAction; // 현재 동작이 종료 동작인지 여부
    private int previousSceneIndex; // 이전 씬의 인덱스 저장 변수

    void Start()
    {
        // UI 요소가 모두 연결되었는지 확인
        if (confirmPanel == null || messageText == null || yesButton == null || noButton == null ||
            backButton == null || homeButton == null)
        {
            Debug.LogError("UI 요소가 연결되지 않았습니다. 유니티 에디터에서 모든 요소를 확인하세요.");
            return;
        }

        // 버튼 클릭 이벤트 리스너 설정
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);

        // 패널 초기 비활성화
        confirmPanel.SetActive(false);

        // 이전 씬의 인덱스 설정
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }

    void OnHomeButtonClick()
    {
        ShowConfirmPanel("게임을 종료하시겠습니까?");
        isExitAction = true; // 종료 동작으로 설정
    }

    void OnBackButtonClick()
    {
        ShowConfirmPanel("이전 화면으로 돌아가시겠습니까?");
        isExitAction = false; // 종료 동작 아님으로 설정
    }

    void ShowConfirmPanel(string message)
    {
        // 메시지 설정
        messageText.text = message;
        // 확인 패널 활성화
        confirmPanel.SetActive(true);
    }

    void OnYesButtonClick()
    {
        confirmPanel.SetActive(false); // 확인 패널 비활성화
        // Unity의 Application.Quit() 메서드를 사용하여 게임 종료
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
            // 이전 씬으로 전환
            if (previousSceneIndex >= 0)
            {
                SceneManager.LoadScene(previousSceneIndex); // 이전 씬으로 전환
            }
            else
            {
                Debug.LogWarning("이전 씬이 설정되지 않았습니다.");
            }
        }
    }

    void OnNoButtonClick()
    {
        confirmPanel.SetActive(false); // 확인 패널 비활성화
    }
}
