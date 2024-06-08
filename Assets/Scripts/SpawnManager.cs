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
        // 일정 시간마다 몬스터를 생성하는 코루틴 시작
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // 일정 시간 대기

            // 랜덤한 위치에서 몬스터 생성
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
            Instantiate(monsterPrefab, spawnPosition, spawnPoint.rotation);

            Debug.Log("Monster Spawned at: " + spawnPosition); // 몬스터가 생성된 위치 로그에 출력
        }
    }
}
