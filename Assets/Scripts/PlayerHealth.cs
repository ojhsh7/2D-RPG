using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float maxHP = 100f; // 최대 HP
    private float currentHP; // 현재 HP

    public CameraShake cameraShake; // 카메라 흔들리기 컴포넌트

    void Start()
    {
        currentHP = maxHP;
 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f); // 테스트용으로 Space 키를 누르면 10의 데미지를 받음
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP); // HP가 음수가 되지 않도록 클램프



        if (cameraShake != null)
        {
            cameraShake.Shake(); // 카메라 흔들리기 호출
        }
    }
}
