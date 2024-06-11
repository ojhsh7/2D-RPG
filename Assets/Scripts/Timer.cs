using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float startTime;

    void Start()
    {
        // ������ ���۵� �� �ð��� ����մϴ�.
        startTime = Time.time;
    }

    void Update()
    {
        // ���� �ð��� ����մϴ�.
        float t = Time.time - startTime;

        // �а� �ʸ� ����մϴ�.
        int minutes = Mathf.FloorToInt(t / 60F);
        int seconds = Mathf.FloorToInt(t % 60F);

        // Ÿ�̸� �ؽ�Ʈ�� ������Ʈ�մϴ�.
        timerText.text = string.Format("{0:0��}:{1:00��}", minutes, seconds);
    }
}
