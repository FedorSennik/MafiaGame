using UnityEngine;
using System.Collections.Generic;

public class RoomObjectSpawner : MonoBehaviour
{
    [Header("Объекты для спавна")]
    [SerializeField] private GameObject[] objectsToSpawn;

    [Header("Места для спавна (GameObjects)")]
    [SerializeField] private GameObject[] spawnPointObjects;

    // Храним ссылки на заспавненные объекты, если нужно удалять вручную
    private List<GameObject> spawnedObjects = new List<GameObject>();

    public void SpawnObjects()
    {
        
        if (spawnPointObjects.Length == 0 || objectsToSpawn.Length == 0) return;
        for (int i = 0; i < spawnPointObjects.Length && i < objectsToSpawn.Length; i++)
        {
            if (spawnPointObjects[i] != null && objectsToSpawn[i] != null)
            {
                Transform spawnTransform = spawnPointObjects[i].transform;

                // Делаем предмет дочерним объектом комнаты
                GameObject obj = Instantiate(objectsToSpawn[i], spawnTransform.position, spawnTransform.rotation, transform);

                spawnedObjects.Add(obj);
            }
        }
    }

    // Если нужно удалять вручную (например, если предметы не дочерние)
    public void DestroySpawnedObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();
    }
}