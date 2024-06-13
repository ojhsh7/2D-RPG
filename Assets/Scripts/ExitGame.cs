using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton; // Exit 버튼 연결
    public Button backButton; // 되돌아가기 버튼 연결
    public Button yesButton; // 예 버튼 연결
    public Button noButton; // 아니오 버튼 연결
    public GameObject messagePanel; // 메시지 패널 연결
    public GameObject confirmPanel; // 확인 패널 연결
    public Text messageText; // 메시지 텍스트 연결
    public Text confirmText; // 확인 텍스트 연결

    private int previousSceneIndex; // 이전 씬의 인덱스 저장 변수
    private bool isExitAction; // 현재 동작이 종료 동작인지 여부

    void Start()
    {
        // UI 요소가 모두 연결되었는지 확인
        if (exitButton == null || backButton == null || yesButton == null || noButton == null ||
            messagePanel == null || confirmPanel == null || messageText == null || confirmText == null)
        {
            Debug.LogError("UI 요소가 연결되지 않았습니다. 유니티 에디터에서 모든 요소를 확인하세요.");
            return;
        }

        // 버튼 클릭 이벤트 리스너 설정
        exitButton.onClick.AddListener(OnExitButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);

        // 패널 초기 비활성화
        messagePanel.SetActive(false);
        confirmPanel.SetActive(false);

        // 이전 씬의 인덱스 설정 (시작 씬을 고려하여 설정 필요)
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }

    void OnExitButtonClick()
    {
        confirmText.text = "게임을 종료하시겠습니까?"; // 확인 메시지 설정
        confirmPanel.SetActive(true); // 확인 패널 활성화
        isExitAction = true; // 종료 동작으로 설정
    }

    void OnBackButtonClick()
    {
        confirmText.text = "이전 화면으로 돌아가시겠습니까?"; // 확인 메시지 설정
        confirmPanel.SetActive(true); // 확인 패널 활성화
        isExitAction = false; // 종료 동작 아님으로 설정
    }

    void OnYesButtonClick()
    {
        if (isExitAction)
        {
            messageText.text = "게임을 종료합니다."; // 종료 메시지 설정
            messagePanel.SetActive(true); // 메시지 패널 활성화
            StartCoroutine(ExitAndLoadStartScene());
        }
        else
        {
            messageText.text = "이전 화면으로 돌아갑니다."; // 이전 화면 메시지 설정
            messagePanel.SetActive(true); // 메시지 패널 활성화
            StartCoroutine(BackAndLoadPreviousScene());
        }
        confirmPanel.SetActive(false); // 확인 패널 비활성화
    }

    void OnNoButtonClick()
    {
        confirmPanel.SetActive(false); // 확인 패널 비활성화
    }

    IEnumerator ExitAndLoadStartScene()
    {
        yield return new WaitForSeconds(2); // 2초 대기
        SceneManager.LoadScene("StartScene"); // StartScene으로 전환
    }

    IEnumerator BackAndLoadPreviousScene()
    {
        yield return new WaitForSeconds(2); // 2초 대기
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
