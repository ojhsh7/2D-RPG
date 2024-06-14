using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject monsterPrefab; // 생성할 몬스터 프리팹
    public Transform[] spawnPoints; // 몬스터를 생성할 위치들
    public float spawnInterval = 10f; // 몬스터를 생성하는 간격 (초 단위)

    void Start()
    {
        // 필요한 컴포넌트가 할당되었는지 확인
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

        // 일정 시간마다 몬스터를 생성하는 코루틴 시작
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // 일정 시간 대기

            // 유효한 스폰 포인트만 사용
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
                // 랜덤한 위치에서 몬스터 생성
                int spawnIndex = Random.Range(0, validSpawnPoints.Count);
                Transform spawnPoint = validSpawnPoints[spawnIndex];
                Vector3 spawnPosition = spawnPoint.position;

                if (spawnPoint != null) // 여기서 null 체크를 한번 더 추가
                {
                    Instantiate(monsterPrefab, spawnPosition, spawnPoint.rotation);
                    Debug.Log("Monster Spawned at: " + spawnPosition); // 몬스터가 생성된 위치 로그에 출력
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
