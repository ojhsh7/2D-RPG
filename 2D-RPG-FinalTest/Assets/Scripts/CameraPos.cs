using UnityEngine;

public class CameraPos : MonoBehaviour
{
    private GameObject playerObj;

    void Update()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            // 플레이어의 위치를 받아서 해당 위치로 카메라를 이동시킵니다.
            Vector3 playerPosition = playerObj.transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        }
    }
}
