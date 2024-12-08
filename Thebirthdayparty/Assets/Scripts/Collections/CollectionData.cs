using UnityEngine;

[CreateAssetMenu(fileName = "New Collection Item", menuName = "Collections/Collection Item")]
public class CollectionData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;        // 小图标（网格视图）
    public Sprite itemFullImage;   // 大图（详情视图）
    [TextArea(2, 5)]
    public string descriptionL1;   // 一级描述
    [TextArea(5, 10)]
    public string descriptionL2;   // 二级描述
    public bool isUnlocked;
    public string unlockCondition; // 解锁条件描述
}