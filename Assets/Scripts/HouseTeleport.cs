using TMPro;
using UnityEngine;

public class HouseTeleport : MonoBehaviour
{
    [Header("Настройки дома")]
    public GameObject roomPrefab;
    [SerializeField] private Transform roomSpawnPoint;
    public float stayTime = 5f;

    [Header("Ссылки")]
    public Transform player;
    public TextMeshProUGUI hintText;

    private GameObject currentRoom;
    private Transform roomPlayerSpawn;
    private Vector3 houseEntrancePos;
    private bool canEnter;
    private bool entranceSet;

    private float teleportCooldown = 8f;
    private float lastTeleportTime = -Mathf.Infinity;

    // Новые переменные
    private bool inRoom = false;
    private float roomExitTime = 0f;

    void Awake()
    {
        if (roomSpawnPoint == null) roomSpawnPoint = GameObject.Find("SpawnPointRoom").transform;
        if (player == null) player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (inRoom)
        {
            float remaining = Mathf.Max(0f, roomExitTime - Time.time);
            if (hintText != null)
                hintText.text = $"Вы в комнате\nОсталось: {remaining:F1} сек.";

            return; // пока в комнате — остальное не обрабатываем
        }

        if (!canEnter) return;

        float remainingCooldown = GetRemainingCooldown();
        UpdateHintText(remainingCooldown);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (remainingCooldown <= 0f)
            {
                EnterRoom();
                lastTeleportTime = Time.time;
            }
            else
            {
                Debug.Log("КД ещё не прошёл");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform != player) return;

        canEnter = true;

        if (!entranceSet)
        {
            houseEntrancePos = player.position;
            entranceSet = true;
        }

        UpdateHintText(GetRemainingCooldown());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform != player) return;

        canEnter = false;
        HideHint();
    }

    void EnterRoom()
    {
        HideHint();

        currentRoom = Instantiate(roomPrefab, roomSpawnPoint.position, roomSpawnPoint.rotation);
        roomPlayerSpawn = currentRoom.transform.Find("SpawnPoint");

        player.position = roomPlayerSpawn != null
            ? roomPlayerSpawn.position
            : currentRoom.transform.position + Vector3.up * 1.5f;

        // Запоминаем время выхода
        inRoom = true;
        roomExitTime = Time.time + stayTime;

        Invoke(nameof(ExitRoom), stayTime);
    }

    void ExitRoom()
    {
        player.position = houseEntrancePos;
        if (currentRoom != null) Destroy(currentRoom);

        inRoom = false;
        HideHint();
    }

    // ===== UI =====
    void UpdateHintText(float remainingCooldown)
    {
        if (hintText == null) return;

        if (remainingCooldown <= 0f)
            hintText.text = $"Нажмите F, чтобы войти\nВремя: {stayTime} сек.";
        else
            hintText.text = $"Подождите {remainingCooldown:F1} сек. до входа";
    }

    void HideHint()
    {
        if (hintText != null) hintText.text = "";
    }

    // ===== Utils =====
    float GetRemainingCooldown()
    {
        return Mathf.Max(0f, (lastTeleportTime + teleportCooldown) - Time.time);
    }
}