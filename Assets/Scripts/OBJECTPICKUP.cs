using UnityEngine;
using static PlayerInteractor;

[RequireComponent(typeof(Collider))]
public class OBJECTPICKUP : MonoBehaviour, IInteractable
{
    [Header("���������")]
    [SerializeField] public ObjectStats stats;

    public void Interact()
    {
        PlayerStats.Instance.ChangeStealMoney(stats.price);
        Destroy(gameObject);
    }
}