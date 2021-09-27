using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    PlayerManager playerManager;
    WeaponSlot equippedSlot;
    DamageCollider weaponDamageCollider;

    public WeaponSlot[] weaponSlots;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
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
        playerManager.isAttacking = false;
    }

    #endregion
}
