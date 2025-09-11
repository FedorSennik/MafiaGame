using UnityEngine;
using UnityEngine.UI;

public class PickableByHold : MonoBehaviour
{
    private float holdTime = 0f;
    private float requiredHoldDuration;

    [SerializeField] private Camera cam;
    [SerializeField] private Slider pickupSlider;

    public ObjectStats stats;


    void Start()
    {
        if (cam == null)
            cam = FindObjectOfType<Camera>();

        if (pickupSlider != null)
            pickupSlider.gameObject.SetActive(false);

        requiredHoldDuration = stats.timeToPickup;
    }

    void Update()
    {
        if (cam == null) return;

        if (Input.GetKey(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.collider.gameObject == gameObject)
            {
                holdTime += Time.deltaTime;

                if (pickupSlider != null)
                {
                    pickupSlider.gameObject.SetActive(true);
                    pickupSlider.value = holdTime / requiredHoldDuration;
                }

                if (holdTime >= requiredHoldDuration)
                {
                    Pickup();
                }
            }
            else
            {
                ResetHold();
            }
        }
        else
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        holdTime = 0f;

        if (pickupSlider != null)
        {
            pickupSlider.value = 0f;
            pickupSlider.gameObject.SetActive(false);
        }
    }

    private void Pickup()
    {
        if (pickupSlider != null)
        {
            pickupSlider.value = 1f;
            pickupSlider.gameObject.SetActive(false);
        }

        PlayerStats.Instance.ChangeStealMoney(stats.price);

        Destroy(gameObject);
    }
}