using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton; // Exit 버튼 연결
    public Button backButton; // 되돌아가기 버튼 연결
    public GameObject messagePanel; // 메시지 패널 연결
    public Text messageText; // 메시지 텍스트 연결

    private int previousSceneIndex; // 이전 씬의 인덱스 저장 변수

    void Start()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
        messagePanel.SetActive(false); // 시작 시 메시지 패널 비활성화

        // 이전 씬의 인덱스 설정 (시작 씬을 고려하여 설정 필요)
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
    }

    void OnExitButtonClick()
    {
        messageText.text = "게임을 종료합니다."; // 메시지 설정
        messagePanel.SetActive(true); // 메시지 패널 활성화
        StartCoroutine(ExitAndLoadStartScene());
    }

    void OnBackButtonClick()
    {
        messageText.text = "이전 화면으로 돌아갑니다."; // 메시지 설정
        messagePanel.SetActive(true); // 메시지 패널 활성화
        StartCoroutine(BackAndLoadPreviousScene());
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
