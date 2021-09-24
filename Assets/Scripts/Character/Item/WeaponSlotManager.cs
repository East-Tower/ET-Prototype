using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponSlot equippedSlot;

    public WeaponSlot[] weaponSlots;

    DamageCollider weaponDamageCollider;

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
        LoadWeaponDamageCollider();
    }

    #region Handle Weapon's Damage Collider

    private void LoadWeaponDamageCollider() 
    {
        weaponDamageCollider = equippedSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void OpenWeaponDamageCollider() 
    {
        weaponDamageCollider.EnableDamageCollider();
    }

    private void CloseWeaponDamageCollider()
    {
        weaponDamageCollider.DisableDamageCollider();
    }

    private void AttackOver() 
    {
        weaponDamageCollider.AttackOver();
    }

    #endregion
}
