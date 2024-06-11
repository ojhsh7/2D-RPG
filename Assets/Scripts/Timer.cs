using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float startTime;

    void Start()
    {
        // 게임이 시작될 때 시간을 기록합니다.
        startTime = Time.time;
    }

    void Update()
    {
        // 현재 시간을 계산합니다.
        float t = Time.time - startTime;

        // 분과 초를 계산합니다.
        int minutes = Mathf.FloorToInt(t / 60F);
        int seconds = Mathf.FloorToInt(t % 60F);

        // 타이머 텍스트를 업데이트합니다.
        timerText.text = string.Format("{0:0분}:{1:00초}", minutes, seconds);
    }
}
