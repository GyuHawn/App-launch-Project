using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemScript : MonoBehaviour
{
    public GameObject[] items; // 생성 아이템
    public int spawnNum; // 생성 아이템 수

    // 맵1
    public GameObject spwanPoint;
    public Vector3 boxSize;

    // 맵2
    public GameObject[] spwanPoints;

    private List<GameObject> currentSpawnedItems = new List<GameObject>(); // 생성된 아이템

    void Start()
    {
        spawnNum = 3;
        if(SceneManager.GetActiveScene().name == "Main")
        {
            SpawnItems();
        }
        else if(SceneManager.GetActiveScene().name == "Main1")
        {
            SpawnItems();
        }
    }

    void Update()
    {
        // 아이템 획득시 재 생성
        if (currentSpawnedItems.Count < spawnNum)
        {
            SpawnItems();
        }
    }

    // 아이템 생성
    private void SpawnItems()
    {
        while (currentSpawnedItems.Count < spawnNum)
        {
            Vector3 spawnPosition = GetSpawnPosition(); // 생성할 위치

            if (spawnPosition != Vector3.zero)
            {
                int randomIndex = Random.Range(0, items.Length); // 랜덤 아이템 선택
                GameObject spawnedItem = Instantiate(items[randomIndex], spawnPosition, Quaternion.Euler(-90, 0, 0)); // 아이템 생성
                currentSpawnedItems.Add(spawnedItem); // 생성된 아이템 리스트 추가
            }
        }
    }

    // 생성할 위치 반환
    private Vector3 GetSpawnPosition()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            // 생성 가능한 위치 찾기
            for (int i = 0; i < 100; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-boxSize.x / 2, boxSize.x / 2), Random.Range(-boxSize.y / 2, boxSize.y / 2), Random.Range(-boxSize.z / 2, boxSize.z / 2)) + spwanPoint.transform.position;

                // 생성할 위치 검사
                if (IsPositionValid(spawnPosition))
                {
                    return spawnPosition;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main1")
        {
            // 랜덤하게 spawnPoint 선택
            GameObject randomSpawnPoint = spwanPoints[Random.Range(0, spwanPoints.Length)];

            // 생성할 위치 검사
            if (IsPositionValid(randomSpawnPoint.transform.position))
            {
                return randomSpawnPoint.transform.position;
            }
        }

        return Vector3.zero;
    }

    // 생성할 위치가 유효한지 검사
    private bool IsPositionValid(Vector3 position)
    {
        // 현재 생성된 아이템들과의 거리 확인
        foreach (var item in currentSpawnedItems)
        {
            // 거리가 5이상인지 확인
            if (Vector3.Distance(item.transform.position, position) < 5)
            {
                return false;
            }

            // 같은 위치인지 확인
            if (item.transform.position == position)
            {
                return false;
            }
        }

        return true;
    }

    // 아이템이 파괴될 때 호출
    public void OnItemDestroyed(GameObject item)
    {
        currentSpawnedItems.Remove(item); // 파괴된 아이템 리스트에서 제거
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnPoints.transform.position, boxSize);
    }*/
}
