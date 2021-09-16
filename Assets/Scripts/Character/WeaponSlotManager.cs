using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponSlot equippedSlot;

    public WeaponSlot[] weaponSlots;
    private void Awake()
    {
        weaponSlots = GetComponentsInChildren<WeaponSlot>();
        foreach (WeaponSlot weapon in weaponSlots) 
        {
            equippedSlot = weapon;
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem) 
    {
        equippedSlot.LoadWeaponModel(weaponItem);
    }

}
