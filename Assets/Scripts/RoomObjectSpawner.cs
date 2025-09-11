using UnityEngine;
using System.Collections.Generic;

public class RoomObjectSpawner : MonoBehaviour
{
    [Header("������� ��� ������")]
    [SerializeField] private GameObject[] objectsToSpawn;

    [Header("����� ��� ������ (GameObjects)")]
    [SerializeField] private GameObject[] spawnPointObjects;

    // ������ ������ �� ������������ �������, ���� ����� ������� �������
    private List<GameObject> spawnedObjects = new List<GameObject>();

    public void SpawnObjects()
    {
        
        if (spawnPointObjects.Length == 0 || objectsToSpawn.Length == 0) return;
        for (int i = 0; i < spawnPointObjects.Length && i < objectsToSpawn.Length; i++)
        {
            if (spawnPointObjects[i] != null && objectsToSpawn[i] != null)
            {
                Transform spawnTransform = spawnPointObjects[i].transform;

                // ������ ������� �������� �������� �������
                GameObject obj = Instantiate(objectsToSpawn[i], spawnTransform.position, spawnTransform.rotation, transform);

                spawnedObjects.Add(obj);
            }
        }
    }

    // ���� ����� ������� ������� (��������, ���� �������� �� ��������)
    public void DestroySpawnedObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();
    }
}