using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem weaponItem;

    public WeaponSlot equippedSlot;
    public DamageCollider weaponDamageCollider;

    private void Awake()
    {
        WeaponSlot[] weaponSlots = GetComponentsInChildren<WeaponSlot>();
        foreach (WeaponSlot weapon in weaponSlots)
        {
            equippedSlot = weapon;
        }
    }
    private void Start()
    {
        if (weaponItem != null)
        {
            LoadWeaponOnSlot(weaponItem);
        }
    }
    public void LoadWeaponOnSlot(WeaponItem weaponItem)
    {
        equippedSlot.LoadWeaponModel(weaponItem);
        LoadWeaponDamageCollider();
    }

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

    }
}