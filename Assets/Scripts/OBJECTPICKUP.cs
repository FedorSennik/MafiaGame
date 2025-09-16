using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class OBJECTPICKUP : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private ObjectStats stats;        // Время и цена подъёма
    [SerializeField] private float maxDistance = 100f; // Максимальная дистанция луча

    [Header("UI")]
    [SerializeField] public Slider pickupSlider;      // Привязать через инспектор или найти на Awake

    public Camera cam;
    private float holdTime;
    private float requiredHoldDuration;
    private bool isHolding;

    private void Awake()
    {
        cam = Camera.main;
        requiredHoldDuration = stats != null ? stats.timeToPickup : 0f;

        if (pickupSlider == null && PickUpSlider.instance != null)
            pickupSlider = PickUpSlider.instance.pickUpSkider;

        if (pickupSlider != null)
            pickupSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (cam == null || stats == null || pickupSlider == null)
            return;

        if (Input.GetKey(KeyCode.E))
            HandleHolding();
        else if (isHolding)
            ResetHold();
    }

    private void HandleHolding()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance) &&
            hit.collider.gameObject == gameObject)
        {
            if (!isHolding)
                isHolding = true;

            holdTime += Time.deltaTime;
            float normalized = Mathf.Clamp01(holdTime / requiredHoldDuration);

            pickupSlider.value = normalized;
            pickupSlider.gameObject.SetActive(true);

            if (holdTime >= requiredHoldDuration)
                CompletePickup();
        }
        else if (isHolding)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTime = 0f;
        pickupSlider.value = 0f;
        pickupSlider.gameObject.SetActive(false);
    }

    private void CompletePickup()
    {
        ResetHold();

        PlayerStats.Instance.ChangeStealMoney(stats.price);

        Destroy(gameObject);
    }
}