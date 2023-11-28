using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ŀ����
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemDatas : ScriptableObject
{
    public enum ItemType
    {
        Sword,      //�� 
        Target,     //Ÿ��
        Random,     //����
        Front,      //����Ʈ
        Lightnig,   //����
        Spike,      //������ũ
        Glove,      //�۷���
        Shoe,       //�Ź�
        Shield,     //����
        Explosion,  //����
        Heal,       //��

    }

    [Header("# Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;

    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level")]
    public float baseDamage;        //�⺻ ������
    public int baseCount;           //�⺻(�ٰŸ� - ����, ���Ÿ� - ����)
    public float[] damages;         //�߰� ������
    public int[] counts;            //�߰�(�ٰŸ� - ����, ���Ÿ� - ����)

    [Header("# Weapon")]
    public GameObject projectile;   //����ü
}
