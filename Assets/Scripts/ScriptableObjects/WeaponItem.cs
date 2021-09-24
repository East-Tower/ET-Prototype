using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isEquipped;

    [Header("攻击动画")]
    public string SW_Regular_Attack_1;
    public string SW_Regular_Attack_2;
    public string SW_Regular_Attack_3;
    public string SW_Regular_Attack_4;
    public string SW_Special_Attack_1;
    public string SW_Special_Attack_2;
    public string SW_Special_Attack_3;
    public string SW_Special_Attack_4;
}
