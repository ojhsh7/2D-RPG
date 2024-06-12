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
            // �÷��̾��� ��ġ�� �޾Ƽ� �ش� ��ġ�� ī�޶� �̵���ŵ�ϴ�.
            Vector3 playerPosition = playerObj.transform.position;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        }
    }
}
