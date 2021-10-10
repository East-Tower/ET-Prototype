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

    private void LoadWeaponDamageCollider() //读取当前所使用的武器
    {
        weaponDamageCollider = equippedSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void OpenWeaponDamageCollider() //在动画器中开启对应武器的碰撞器
    {
        weaponDamageCollider.EnableDamageCollider();
    }

    private void CloseWeaponDamageCollider() //在动画器中关闭对应武器的碰撞器
    {
        weaponDamageCollider.DisableDamageCollider();
    }

    private void AttackOver() //确定何时提前关闭玩家当前的攻击状态
    {
        playerManager.isAttacking = false; 
    }

    #endregion
}
