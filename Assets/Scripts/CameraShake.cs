using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.1f; // 카메라 흔들림 강도

    private Vector3 originalPosition; // 원래 카메라 위치

    void Start()
    {
        originalPosition = transform.position;
    }

    public void Shake()
    {
        // 흔들림 효과를 랜덤하게 생성하여 카메라 위치에 더합니다.
        Vector3 shakePosition = originalPosition + Random.insideUnitSphere * shakeAmount;
        transform.position = shakePosition;
    }
}
