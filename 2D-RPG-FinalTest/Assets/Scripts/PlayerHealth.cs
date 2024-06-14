using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float maxHP = 100f; // �ִ� HP
    private float currentHP; // ���� HP

    public CameraShake cameraShake; // ī�޶� ��鸮�� ������Ʈ

    void Start()
    {
        currentHP = maxHP;
 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f); // �׽�Ʈ������ Space Ű�� ������ 10�� �������� ����
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP); // HP�� ������ ���� �ʵ��� Ŭ����



        if (cameraShake != null)
        {
            cameraShake.Shake(); // ī�޶� ��鸮�� ȣ��
        }
    }
}
