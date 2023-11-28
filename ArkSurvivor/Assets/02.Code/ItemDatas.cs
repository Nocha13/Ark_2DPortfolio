using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에셋 커스텀
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemDatas : ScriptableObject
{
    public enum ItemType
    {
        Sword,      //검 
        Target,     //타겟
        Random,     //랜덤
        Front,      //프론트
        Lightnig,   //낙뢰
        Spike,      //스파이크
        Glove,      //글러브
        Shoe,       //신발
        Shield,     //쉴드
        Explosion,  //전멸
        Heal,       //힐

    }

    [Header("# Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;

    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level")]
    public float baseDamage;        //기본 데미지
    public int baseCount;           //기본(근거리 - 갯수, 원거리 - 관통)
    public float[] damages;         //추가 데미지
    public int[] counts;            //추가(근거리 - 갯수, 원거리 - 관통)

    [Header("# Weapon")]
    public GameObject projectile;   //투사체
}
