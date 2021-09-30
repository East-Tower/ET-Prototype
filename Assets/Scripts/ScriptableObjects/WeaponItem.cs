using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isEquipped;

    [Header("普通连招")]
    public Skill[] regularSkills;

    [Header("特殊连招")]
    public Skill[] specialSkills;
}
