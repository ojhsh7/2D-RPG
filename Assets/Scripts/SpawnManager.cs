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
        // �ʿ��� ������Ʈ�� �Ҵ�Ǿ����� Ȯ��
        if (monsterPrefab == null)
        {
            Debug.LogError("Monster prefab is not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn points are not assigned or empty!");
            return;
        }

        // ���� �ð����� ���͸� �����ϴ� �ڷ�ƾ ����
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // ���� �ð� ���

            // ��ȿ�� ���� ����Ʈ�� ���
            List<Transform> validSpawnPoints = new List<Transform>();
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    validSpawnPoints.Add(spawnPoint);
                }
            }

            if (validSpawnPoints.Count > 0)
            {
                // ������ ��ġ���� ���� ����
                int spawnIndex = Random.Range(0, validSpawnPoints.Count);
                Transform spawnPoint = validSpawnPoints[spawnIndex];
                Vector3 spawnPosition = spawnPoint.position;

                if (spawnPoint != null) // ���⼭ null üũ�� �ѹ� �� �߰�
                {
                    Instantiate(monsterPrefab, spawnPosition, spawnPoint.rotation);
                    Debug.Log("Monster Spawned at: " + spawnPosition); // ���Ͱ� ������ ��ġ �α׿� ���
                }
                else
                {
                    Debug.LogWarning("Chosen spawn point is null.");
                }
            }
            else
            {
                Debug.LogWarning("No valid spawn points available!");
            }
        }
    }
}
