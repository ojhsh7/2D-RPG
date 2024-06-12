using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.1f; // ī�޶� ��鸲 ����

    private Vector3 originalPosition; // ���� ī�޶� ��ġ

    void Start()
    {
        originalPosition = transform.position;
    }

    public void Shake()
    {
        // ��鸲 ȿ���� �����ϰ� �����Ͽ� ī�޶� ��ġ�� ���մϴ�.
        Vector3 shakePosition = originalPosition + Random.insideUnitSphere * shakeAmount;
        transform.position = shakePosition;
    }
}
