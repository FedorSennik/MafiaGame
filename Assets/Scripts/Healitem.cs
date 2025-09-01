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
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&  !isUsed && isEquiped)
        {
            playerStats.Heal(50);
            isUsed = true;
        }
    }
    public void OnEquip()
    {
        isEquiped = true;
    }

    public void OnUnEquip()
    {
        isEquiped = false;
    }

    public void OnAdd()
    {
        isAdded = true;
        isDropped = false;
        Finder();
    }

    
    public void Finder()
    {
        healPerson = transform.root.gameObject;
        playerStats = healPerson.GetComponent<PlayerStats>();
    }
}