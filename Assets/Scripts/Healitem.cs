using Unity.VisualScripting;
using UnityEngine;

public class HealItem : Equipment, IEquipment
{
    [SerializeField]private GameObject healPerson;
    [SerializeField]private PlayerStats playerStats;

    [SerializeField] private bool isUsed;

    private void Awake()
    {
        isDropped = true;
        isUsed = false;
        isEquiped = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&  !isUsed && isEquiped)
        {
            playerStats.Heal(50);
            isUsed = true;
        }
    }
    public void OnRemove()
    {
        isAdded = false;
        isEquiped = false;
        isDropped = true;
        gameObject.SetActive(true);
        gameObject.AddComponent<Rigidbody>();

    }
    public void OnAdd()
    {
        isAdded = true;
        isEquiped = false;
        isDropped = false;

        Finder();

        gameObject.SetActive(false);


        Destroy(GetComponent<Rigidbody>());
    }

    public void OnEquip()
    {
        isEquiped = true;

        gameObject.SetActive(true);
    }

    public void OnUnEquip()
    {
        isEquiped = false;
        gameObject.SetActive(false);
    }

    public void Finder()
    {
        healPerson = transform.root.gameObject;
        playerStats = healPerson.GetComponent<PlayerStats>();
    }
}