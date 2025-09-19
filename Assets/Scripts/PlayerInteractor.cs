using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Slider pickupSlider;
    [SerializeField] private Camera cam;

    private IInteractable currentInteractable;
    private OBJECTPICKUP currentPickup;
    private float interactTimer;
    private bool isInteracting;

    private void Start()
    {
        pickupSlider.gameObject.SetActive(false);
        pickupSlider.value = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryStartInteraction();
        }

        if (Input.GetKey(KeyCode.E) && isInteracting)
        {
            interactTimer += Time.deltaTime;
            float duration = currentPickup.stats.timeToPickup;
            pickupSlider.value = interactTimer / duration;

            if (interactTimer >= duration)
            {
                currentInteractable.Interact();
                ResetInteraction();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            ResetInteraction();
        }
    }

    private void TryStartInteraction()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10))
        {
            currentPickup = hit.collider.GetComponent<OBJECTPICKUP>();
            currentInteractable = hit.collider.GetComponent<IInteractable>();

            if (currentPickup != null && currentInteractable != null)
            {
                interactTimer = 0f;
                isInteracting = true;

                pickupSlider.value = 0f;
                pickupSlider.gameObject.SetActive(true);
            }
        }
    }

    private void ResetInteraction()
    {
        isInteracting = false;
        interactTimer = 0f;
        currentInteractable = null;
        currentPickup = null;
        pickupSlider.gameObject.SetActive(false);
    }

    public interface IInteractable
    {
        void Interact();
    }
}