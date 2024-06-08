using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ExitGame : MonoBehaviour
{
    public Button exitButton; // Exit 버튼 연결
    public GameObject messagePanel; // 메시지 패널 연결
    public Text messageText; // 메시지 텍스트 연결

    void Start()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
        messagePanel.SetActive(false); // 시작 시 메시지 패널 비활성화
    }

    void OnExitButtonClick()
    {
        messageText.text = "게임을 종료합니다."; // 메시지 설정
        messagePanel.SetActive(true); // 메시지 패널 활성화
        StartCoroutine(ExitAndLoadStartScene());
    }

    IEnumerator ExitAndLoadStartScene()
    {
        yield return new WaitForSeconds(2); // 2초 대기
        SceneManager.LoadScene("StartScene"); // StartScene으로 전환
    }
}

