using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager WeaponSlotManager;

    public WeaponItem equippedItem;

    private void Awake()
    {
        WeaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        WeaponSlotManager.LoadWeaponOnSlot(equippedItem);
    }
}
