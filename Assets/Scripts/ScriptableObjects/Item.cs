using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Regular Item")]
public class Item : ScriptableObject
{
    [Header("道具信息")]
    //public Sprite itemIcon;
    public string itemName;
}
