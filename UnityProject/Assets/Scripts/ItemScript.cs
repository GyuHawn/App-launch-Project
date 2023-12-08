using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemScript : MonoBehaviour
{
    public GameObject[] items; // ���� ������
    public int spawnNum; // ���� ������ ��

    // ��1
    public GameObject spwanPoint;
    public Vector3 boxSize;

    // ��2
    public GameObject[] spwanPoints;

    private List<GameObject> currentSpawnedItems = new List<GameObject>(); // ������ ������

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
        // ������ ȹ��� �� ����
        if (currentSpawnedItems.Count < spawnNum)
        {
            SpawnItems();
        }
    }

    // ������ ����
    private void SpawnItems()
    {
        while (currentSpawnedItems.Count < spawnNum)
        {
            Vector3 spawnPosition = GetSpawnPosition(); // ������ ��ġ

            if (spawnPosition != Vector3.zero)
            {
                int randomIndex = Random.Range(0, items.Length); // ���� ������ ����
                GameObject spawnedItem = Instantiate(items[randomIndex], spawnPosition, Quaternion.Euler(-90, 0, 0)); // ������ ����
                currentSpawnedItems.Add(spawnedItem); // ������ ������ ����Ʈ �߰�
            }
        }
    }

    // ������ ��ġ ��ȯ
    private Vector3 GetSpawnPosition()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            // ���� ������ ��ġ ã��
            for (int i = 0; i < 100; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-boxSize.x / 2, boxSize.x / 2), Random.Range(-boxSize.y / 2, boxSize.y / 2), Random.Range(-boxSize.z / 2, boxSize.z / 2)) + spwanPoint.transform.position;

                // ������ ��ġ �˻�
                if (IsPositionValid(spawnPosition))
                {
                    return spawnPosition;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main1")
        {
            // �����ϰ� spawnPoint ����
            GameObject randomSpawnPoint = spwanPoints[Random.Range(0, spwanPoints.Length)];

            // ������ ��ġ �˻�
            if (IsPositionValid(randomSpawnPoint.transform.position))
            {
                return randomSpawnPoint.transform.position;
            }
        }

        return Vector3.zero;
    }

    // ������ ��ġ�� ��ȿ���� �˻�
    private bool IsPositionValid(Vector3 position)
    {
        // ���� ������ �����۵���� �Ÿ� Ȯ��
        foreach (var item in currentSpawnedItems)
        {
            // �Ÿ��� 5�̻����� Ȯ��
            if (Vector3.Distance(item.transform.position, position) < 5)
            {
                return false;
            }

            // ���� ��ġ���� Ȯ��
            if (item.transform.position == position)
            {
                return false;
            }
        }

        return true;
    }

    // �������� �ı��� �� ȣ��
    public void OnItemDestroyed(GameObject item)
    {
        currentSpawnedItems.Remove(item); // �ı��� ������ ����Ʈ���� ����
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnPoints.transform.position, boxSize);
    }*/
}
