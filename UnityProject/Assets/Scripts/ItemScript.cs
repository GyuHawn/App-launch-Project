using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject[] items; // ���� ������
    public int spawnNum; // ���� ������ ��

    public GameObject spawnPoints;
    public Vector3 boxSize;

    private List<GameObject> currentSpawnedItems = new List<GameObject>(); // ������ ������

    void Start()
    {
        spawnNum = 3;
        SpawnItems();
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
        // ���� ������ ��ġ ã��
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-boxSize.x / 2, boxSize.x / 2), Random.Range(-boxSize.y / 2, boxSize.y / 2), Random.Range(-boxSize.z / 2, boxSize.z / 2)) + spawnPoints.transform.position;

            // ������ ��ġ �˻�
            if (IsPositionValid(spawnPosition))
            {
                return spawnPosition;
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
