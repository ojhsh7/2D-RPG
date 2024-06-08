using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject monsterPrefab; // ������ ���� ������
    public Transform[] spawnPoints; // ���͸� ������ ��ġ��
    public float spawnInterval = 10f; // ���͸� �����ϴ� ���� (�� ����)

    void Start()
    {
        // ���� �ð����� ���͸� �����ϴ� �ڷ�ƾ ����
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // ���� �ð� ���

            // ������ ��ġ���� ���� ����
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            Instantiate(monsterPrefab, spawnPosition, spawnPoint.rotation);

            Debug.Log("Monster Spawned at: " + spawnPosition); // ���Ͱ� ������ ��ġ �α׿� ���
        }
    }
}
